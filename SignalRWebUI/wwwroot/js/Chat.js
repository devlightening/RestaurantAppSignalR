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
    const onlineDepartmentsList = $("#onlineDepartmentsList"); // Yeni eklenen online kullanıcılar alanı
    const loadingOnlineUsers = $("#loadingOnlineUsers"); // Yükleniyor mesajı

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

        const listItem = $(`<li class="message-item">${messageHtml}</li>`); // message-item sınıfını ekledik

        // Kendi gönderdiğimiz mesajları farklı stilize et
        const currentSelectedSenderId = parseInt(senderUserSelect.val());
        const currentSelectedSenderName = senderUserSelect.find(':selected').text().split(' (')[0]; // Tam adı al, departmanı ayır

        // Basit bir kontrol: Eğer seçili gönderen ID'si, gizli currentUserId ile eşleşiyorsa
        // ve gelen mesajın gönderen adı, seçili gönderen adıyla eşleşiyorsa
        if (currentSelectedSenderId === parseInt(currentUserIdInput.val()) && senderFullName === currentSelectedSenderName) {
            listItem.addClass("my-message");
        }

        messagesList.append(listItem);
        messagesList.scrollTop(messagesList[0].scrollHeight); // En alta kaydır
    };

    // Kullanıcıları API'den çekme ve dropdown'ları doldurma
    const loadUsersForDropdowns = async () => {
        try {
            const response = await fetch("https://localhost:7000/api/AppUsers");
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const users = await response.json();

            senderUserSelect.empty();
            receiverUserSelect.find('option:not([value="0"])').remove();

            users.forEach(user => {
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
            const response = await fetch("https://localhost:7000/api/Messages/GetAllMessages");
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const messages = await response.json();
            messagesList.empty(); // Mevcut mesajları temizle
            messages.sort((a, b) => new Date(a.timestamp) - new Date(b.timestamp));

            messages.forEach(msg => {
                addMessageToDisplay(msg.senderFullName, msg.content, msg.timestamp, msg.receiverFullName);
            });
            console.log("Geçmiş mesajlar yüklendi.");
        } catch (err) {
            console.error("Geçmiş mesajlar yüklenirken hata oluştu:", err);
        }
    };

    // Online kullanıcıları departmanlara göre yükleme ve gösterme fonksiyonu
    const loadAndDisplayOnlineUsers = async () => {
        loadingOnlineUsers.show(); // Yükleniyor mesajını göster
        onlineDepartmentsList.empty(); // Mevcut listeyi temizle

        try {
            const response = await fetch("https://localhost:7000/api/AppUsers/online"); // Online kullanıcıları çeken API
            if (!response.ok) {
                throw new Error(`HTTP error! status: ${response.status}`);
            }
            const onlineUsers = await response.json();

            // Departmanları gruplamak için bir obje
            const groupedUsers = {};
            const departmentOrder = ["Yönetici", "Garson", "Mutfak", "Bar", "Kasa", "Temizlik"]; // Departman sıralaması

            onlineUsers.forEach(user => {
                // API'den gelen userDepartment değeri enum string'i olmalı (Program.cs ayarından sonra)
                const departmentName = user.userDepartment;
                if (!groupedUsers[departmentName]) {
                    groupedUsers[departmentName] = [];
                }
                groupedUsers[departmentName].push(user);
            });

            // Departmanları belirlenen sıraya göre döngüye al
            departmentOrder.forEach(departmentName => {
                const usersInDepartment = groupedUsers[departmentName] || [];
                if (usersInDepartment.length > 0) {
                    const departmentGroupDiv = $(`<div class="department-group"></div>`);
                    const departmentHeaderDiv = $(`<div class="department-header"></div>`);
                    departmentHeaderDiv.append(`<h5 class="department-title">${departmentName}</h5>`);
                    departmentHeaderDiv.append(`<span class="online-count">${usersInDepartment.length}</span>`);
                    departmentGroupDiv.append(departmentHeaderDiv);

                    const userListUl = $(`<ul class="user-list"></ul>`);
                    usersInDepartment.forEach(user => {
                        userListUl.append(`
                                <li class="user-list-item">
                                    <span class="online-dot"></span>
                                    <span class="user-name">${user.fullName}</span>
                                </li>
                            `);
                    });
                    departmentGroupDiv.append(userListUl);
                    onlineDepartmentsList.append(departmentGroupDiv);
                }
            });

            if (onlineUsers.length === 0) {
                onlineDepartmentsList.append('<p class="text-center text-muted">Şu anda çevrimiçi kullanıcı bulunmamaktadır.</p>');
            }

            console.log("Çevrimiçi kullanıcılar listesi güncellendi.");

        } catch (err) {
            console.error("Çevrimiçi kullanıcılar yüklenirken hata oluştu:", err);
            onlineDepartmentsList.empty();
            onlineDepartmentsList.append('<p class="text-center text-danger">Çevrimiçi kullanıcılar yüklenirken bir hata oluştu.</p>');
        } finally {
            loadingOnlineUsers.hide(); // Yükleniyor mesajını gizle
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
            await loadAndDisplayOnlineUsers(); // Bağlantı kurulduktan sonra online kullanıcıları yükle
        } catch (err) {
            updateConnectionStatus(signalR.HubConnectionState.Disconnected, "Bağlantı Hatası");
            sendbutton.prop("disabled", true); // Hata durumunda butonu devre dışı bırak
            console.error("SignalR bağlantı hatası:", err.toString());
            // Otomatik yeniden bağlanma devrede olduğu için manuel bir tekrar başlatmaya gerek yok
        }
    };

    // Mesaj almayı dinle (Hub'dan gelen mesajlar)
    connection.on("ReceiveMessage", (senderFullName, messageContent, timestamp, receiverFullName) => {
        addMessageToDisplay(senderFullName, messageContent, timestamp, receiverFullName);
    });

    // Çevrimiçi kullanıcı listesi güncellemesini dinle (Hub'dan gelecek)
    connection.on("ReceiveOnlineUsersUpdate", async () => {
        console.log("Çevrimiçi kullanıcı listesi güncelleme sinyali alındı.");
        await loadAndDisplayOnlineUsers(); // Listeyi yeniden yükle
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
        loadAndDisplayOnlineUsers(); // Yeniden bağlandığında da online kullanıcıları yükle
    });

    connection.onclose(error => {
        updateConnectionStatus(signalR.HubConnectionState.Disconnected, "Bağlantı Kesildi");
        sendbutton.prop("disabled", true); // Bağlantı kesildiğinde butonu devre dışı bırak
        console.error("SignalR bağlantısı kesildi.", error);
        loadAndDisplayOnlineUsers(); // Bağlantı kesildiğinde online listeyi de güncelle (muhtemelen boş olur)
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
