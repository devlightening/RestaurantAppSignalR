$(document).ready(() => {
    // SignalR Hub bağlantısı oluştur
    let connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7000/signalrhub") // Hub URL'inizi kontrol edin!
        .withAutomaticReconnect({ // Otomatik yeniden bağlanma yapılandırması
            nextRetryDelayInMilliseconds: retryContext => {
                if (retryContext.elapsedMilliseconds < 60000) {
                    // İlk 60 saniye içinde 0, 2, 10, ve 30 saniye aralıklarla tekrar dene
                    return [0, 2000, 10000, 30000][retryContext.previousRetryCount];
                }
                // Sonraki tüm denemeler için 30 saniye sonra tekrar dene
                return 30000;
            }
        })
        .build();

    // DOM elementlerini jQuery ile seç
    const senderUserSelect = $("#senderUserSelect");
    const receiverUserSelect = $("#receiverUserSelect");
    const messageinput = $("#messageinput");
    const sendbutton = $("#sendbutton");
    const messagesList = $("#messagesList");
    const connectionStatusDiv = $("#connectionStatus");
    const currentUserIdInput = $("#currentUserId"); // Gizli currentUserId alanı (HTML'de value="1" olarak ayarlı)

    // Başlangıçta gönderme butonunu devre dışı bırak
    sendbutton.prop("disabled", true);

    // Bağlantı durumunu güncelleyen yardımcı fonksiyon
    const updateConnectionStatus = (state, message) => {
        let statusText = "";
        let statusClass = "";

        switch (state) {
            case signalR.HubConnectionState.Connecting:
                statusText = "Bağlanıyor...";
                statusClass = "alert-info";
                break;
            case signalR.HubConnectionState.Connected:
                statusText = "Bağlandı";
                statusClass = "alert-success";
                break;
            case signalR.HubConnectionState.Disconnected:
                statusText = "Bağlantı Kesildi";
                statusClass = "alert-danger";
                break;
            case signalR.HubConnectionState.Reconnecting:
                statusText = "Yeniden Bağlanıyor...";
                statusClass = "alert-warning";
                break;
            default:
                statusText = "Bilinmiyor";
                statusClass = "alert-secondary";
                break;
        }
        connectionStatusDiv.text(`Durum: ${statusText}`).removeClass().addClass(`alert ${statusClass}`);
    };

    // Mesajı listeye ekleyen fonksiyon
    // Yeni format: GönderenFullName: Mesaj (AlıcıFullName) | Saat:Dakika
    // Bu fonksiyon artık doğrudan DTO'daki alan isimlerini bekliyor
    const addMessageToDisplay = (senderFullName, messageContent, timestamp, receiverFullName) => {
        const time = new Date(timestamp);
        const hours = String(time.getHours()).padStart(2, '0');
        const minutes = String(time.getMinutes()).padStart(2, '0');

        let messageHtml = `<strong>${senderFullName}:</strong> ${messageContent}`;
        // Eğer alıcı adı varsa ve genel sohbet değilse (veya gönderen ile aynı değilse)
        if (receiverFullName && receiverFullName !== "Genel Sohbet" && receiverFullName.trim() !== senderFullName.trim()) {
            messageHtml += ` (<span class="receiver-name">${receiverFullName}</span>)`;
        }
        messageHtml += ` <span class="timestamp">${hours}:${minutes}</span>`;

        const listItem = $(`<li class="list-group-item">${messageHtml}</li>`);

        // Kendi gönderdiğimiz mesajları farklı stilize et
        const currentSelectedSenderId = parseInt(senderUserSelect.val());
        const currentSelectedSenderName = senderUserSelect.find(':selected').text().split(' (')[0]; // Tam adı al, departmanı ayır

        // Basit bir kontrol: Eğer seçili gönderen ID'si, gizli currentUserId ile eşleşiyorsa
        // ve gelen mesajın gönderen adı, seçili gönderen adıyla eşleşiyorsa
        // Not: currentUserIdInput.val() değeri, oturum açmış kullanıcının gerçek ID'si olmalıdır.
        if (currentSelectedSenderId === parseInt(currentUserIdInput.val()) && senderFullName === currentSelectedSenderName) {
            listItem.addClass("my-message");
        }

        messagesList.append(listItem);
        messagesList.scrollTop(messagesList[0].scrollHeight); // En alta kaydır
    };

    // Kullanıcıları API'den çekme ve dropdown'ları doldurma
    const loadUsersForDropdowns = async () => {
        try {
            // AppUsersController'ınızdaki GetAll metodunun URL'si
            const response = await fetch("https://localhost:7000/api/AppUsers");
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const users = await response.json();

            senderUserSelect.empty();
            receiverUserSelect.find('option:not([value="0"])').remove();

            users.forEach(user => {
                // ResultAppUserDto'daki 'fullName' ve 'userDepartment' alanlarını kullan
                const fullNameWithDepartment = `${user.fullName} (${user.userDepartment})`;
                senderUserSelect.append($(`<option value="${user.appUserId}">${fullNameWithDepartment}</option>`));
                receiverUserSelect.append($(`<option value="${user.appUserId}">${fullNameWithDepartment}</option>`));
            });

            const defaultSenderId = parseInt(currentUserIdInput.val());
            if (senderUserSelect.find(`option[value="${defaultSenderId}"]`).length) {
                senderUserSelect.val(defaultSenderId);
            } else if (users.length > 0) {
                senderUserSelect.val(users[0].appUserId);
            }

            console.log("Kullanıcılar dropdown'lara yüklendi.");
        } catch (err) {
            console.error("Kullanıcılar yüklenirken hata oluştu:", err);
        }
    };

    // Geçmiş mesajları API'den çekme fonksiyonu
    const loadHistoricalMessages = async () => {
        try {
            // Mesaj API'nizin GetAllMessages endpoint'inin URL'si
            // Bu API'nin ResultMessageDto (MessageId, Content, Timestamp, SenderFullName, ReceiverFullName) döndürdüğünü varsayıyoruz.
            const response = await fetch("https://localhost:7000/api/Messages/GetAllMessages");
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const messages = await response.json();
            messagesList.empty(); // Mevcut mesajları temizle
            // Mesajları en eskiden en yeniye doğru sırala (API'den ters sırada geliyorsa)
            messages.sort((a, b) => new Date(a.timestamp) - new Date(b.timestamp)); // DTO'daki Timestamp'a göre sırala

            messages.forEach(msg => {
                // API'den gelen mesaj objesinin yapısına göre alanları ayarlayın
                // DTO'daki yeni isimler: msg.senderFullName, msg.content, msg.timestamp, msg.receiverFullName
                addMessageToDisplay(msg.senderFullName, msg.content, msg.timestamp, msg.receiverFullName);
            });
            console.log("Geçmiş mesajlar yüklendi.");
        } catch (err) {
            console.error("Geçmiş mesajlar yüklenirken hata oluştu:", err);
        }
    };

    // SignalR bağlantısını başlatma fonksiyonu
    const startConnection = async () => {
        try {
            updateConnectionStatus(signalR.HubConnectionState.Connecting);
            await connection.start();
            updateConnectionStatus(connection.state, "Bağlandı");
            sendbutton.prop("disabled", false); // Bağlantı başarılıysa butonu etkinleştir
            console.log("SignalR bağlantısı başarıyla kuruldu. Bağlantı ID:", connection.connectionId);
            await loadUsersForDropdowns(); // Bağlantı kurulduktan sonra kullanıcıları yükle
            await loadHistoricalMessages(); // Bağlantı kurulduktan sonra geçmiş mesajları yükle
        } catch (err) {
            updateConnectionStatus(signalR.HubConnectionState.Disconnected, "Bağlantı Hatası");
            sendbutton.prop("disabled", true); // Hata durumunda butonu devre dışı bırak
            console.error("SignalR bağlantı hatası:", err.toString());
            // Otomatik yeniden bağlanma devrede olduğu için manuel bir tekrar başlatmaya gerek yok
        }
    };

    // Mesaj almayı dinle (Hub'dan gelen mesajlar)
    // Hub'ınızdan "ReceiveMessage" metodunun 4 parametre (senderFullName, messageContent, timestamp, receiverFullName) gönderdiğini varsayıyorum.
    connection.on("ReceiveMessage", (senderFullName, messageContent, timestamp, receiverFullName) => {
        addMessageToDisplay(senderFullName, messageContent, timestamp, receiverFullName);
    });

    // SUNUCUDAN GELEN HATA MESAJLARINI DİNLE
    connection.on("ReceiveError", (errorMessage) => {
        console.error("Sunucudan Hata:", errorMessage);
        alert("Sunucudan Hata: " + errorMessage); // Kullanıcıya hatayı göster
    });

    // Bağlantı durum değişikliklerini dinle
    connection.onreconnecting(error => {
        updateConnectionStatus(signalR.HubConnectionState.Reconnecting, "Yeniden Bağlanıyor...");
        sendbutton.prop("disabled", true); // Yeniden bağlanırken butonu devre dışı bırak
        console.warn("SignalR yeniden bağlanıyor...", error);
    });

    connection.onreconnected(connectionId => {
        updateConnectionStatus(signalR.HubConnectionState.Connected, "Yeniden Bağlandı");
        sendbutton.prop("disabled", false); // Yeniden bağlandıktan sonra butonu etkinleştir
        console.log("SignalR yeniden bağlandı. Connection ID:", connectionId);
        loadUsersForDropdowns(); // Yeniden bağlandığında da kullanıcıları yükle
        loadHistoricalMessages(); // Yeniden bağlandığında da geçmiş mesajları yükle
    });

    connection.onclose(error => {
        updateConnectionStatus(signalR.HubConnectionState.Disconnected, "Bağlantı Kesildi");
        sendbutton.prop("disabled", true); // Bağlantı kesildiğinde butonu devre dışı bırak
        console.error("SignalR bağlantısı kesildi.", error);
    });

    // Mesaj gönderme butonu tıklama olayı
    sendbutton.on("click", async (event) => {
        event.preventDefault(); // Varsayılan form gönderme davranışını engelle

        const senderUserId = parseInt(senderUserSelect.val());
        const receiverUserId = parseInt(receiverUserSelect.val());
        const messageContent = messageinput.val().trim();

        if (isNaN(senderUserId) || senderUserId <= 0) {
            alert("Lütfen geçerli bir Gönderen seçin.");
            senderUserSelect.focus();
            return;
        }
        if (isNaN(receiverUserId)) { // Receiver ID 0 olabilir (genel sohbet için)
            alert("Lütfen geçerli bir Alıcı seçin.");
            receiverUserSelect.focus();
            return;
        }
        if (!messageContent) {
            alert("Lütfen mesajınızı girin.");
            messageinput.focus();
            return;
        }

        try {
            if (connection.state === signalR.HubConnectionState.Connected) {
                // Hub'a mesajı gönder
                // Hub'daki SendMessage metodu int senderUserId, int receiverUserId, string messageContent almalı
                await connection.invoke("SendMessage", senderUserId, receiverUserId, messageContent);
                messageinput.val(""); // Mesaj gönderildikten sonra mesaj inputunu temizle
                messageinput.focus(); // Mesaj inputuna odaklan
            } else {
                console.warn("SignalR bağlantısı bağlı değil, mesaj gönderilemedi.");
                alert("Bağlantı kurulu değil, lütfen bekleyin veya sayfayı yenileyin.");
            }
        } catch (err) {
            console.error("SendMessage invoke hatası:", err.toString());
            alert("Mesaj gönderilirken bir hata oluştu: " + err.message);
        }
    });

    // Mesaj inputunda Enter tuşuna basıldığında mesaj gönderme
    messageinput.on("keypress", (e) => {
        if (e.which === 13 && !e.shiftKey) { // Enter tuşu ve Shift tuşu basılı değilse
            e.preventDefault(); // Varsayılan Enter davranışını engelle (yeni satır)
            sendbutton.click(); // Gönderme butonuna tıkla
        }
    });

    // Sayfa yüklendiğinde bağlantıyı başlat
    startConnection();
});
