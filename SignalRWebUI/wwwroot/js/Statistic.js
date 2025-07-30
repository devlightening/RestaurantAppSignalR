$(document).ready(() => {
    let connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7000/signalrhub")
        .withAutomaticReconnect({
            nextRetryDelayInMilliseconds: retryContext => {
                if (retryContext.elapsedMilliseconds < 60000) {
                    // Retry after 0, 2, 10, and 30 seconds
                    return [0, 2000, 10000, 30000][retryContext.previousRetryCount];
                }
                // Sonraki tüm denemeler için 30 saniye sonra tekrar dene
                return 30000;
            }
        })
        .build();

    let statisticInterval; // setInterval referansını tutmak için

    const startConnection = async () => {
        try {
            $("#connstatus").text(connection.state); // Bağlantı durumu güncelleniyor
            await connection.start();
            $("#connstatus").text(connection.state);
            $("#connstatus").removeClass("pulse");
            console.log("SignalR bağlantısı başarıyla kuruldu. Bağlantı ID:", connection.connectionId);

            // Bağlantı başarılı olduğunda istatistikleri gönderme intervalini başlat
            if (statisticInterval) clearInterval(statisticInterval); // Önceki intervali temizle
            statisticInterval = setInterval(async () => {
                try {
                    if (connection.state === signalR.HubConnectionState.Connected) {
                        await connection.invoke("SendStatistic");
                    } else {
                        console.warn("SignalR bağlantısı bağlı değil, SendStatistic çağrısı atlandı.");
                    }
                } catch (err) {
                    console.error("SignalR invoke error (SendStatistic):", err);
                    $("#connstatus").addClass("pulse");
                }
            }, 1000); // Her 1 saniyede bir istatistikleri gönder

        } catch (err) {
            console.error("SignalR connection error:", err);
            $("#connstatus").text("Bağlantı Hatası");
            $("#connstatus").addClass("pulse");
            // Bağlantı hatası durumunda intervali temizle
            if (statisticInterval) clearInterval(statisticInterval);
        }
    };

    // Bağlantı durum değişikliklerini dinle
    connection.onreconnecting(error => {
        console.warn("SignalR yeniden bağlanıyor...", error);
        $("#connstatus").text("Yeniden Bağlanıyor");
        $("#connstatus").addClass("pulse");
        if (statisticInterval) clearInterval(statisticInterval); // Yeniden bağlanırken intervali durdur
    });

    connection.onreconnected(connectionId => {
        console.log("SignalR yeniden bağlandı. Connection ID:", connectionId);
        $("#connstatus").text(connection.state);
        $("#connstatus").removeClass("pulse");
        // Yeniden bağlandıktan sonra intervali tekrar başlat
        startConnection(); // Bu, intervali yeniden başlatır ve istatistikleri çekmeye devam eder
    });

    connection.onclose(error => {
        console.error("SignalR bağlantısı kesildi.", error);
        $("#connstatus").text("Bağlantı Kesildi");
        $("#connstatus").addClass("pulse");
        if (statisticInterval) clearInterval(statisticInterval); // Bağlantı kesildiğinde intervali durdur
        // Otomatik yeniden bağlanma zaten HubConnectionBuilder'da yapılandırıldı
    });

    // İlk bağlantıyı başlat
    startConnection();

    // İstatistik alıcıları (Receive metotları)
    connection.on("ReceiveCategoryCount", value => {
        console.log("Received Category Count:", value); // Konsola yazdır
        $("#categorycount").text(value);
        animateCard("#categorycount");
    });
    connection.on("ReceiveActiveCategoryCount", value => {
        console.log("Received Active Category Count:", value); // Konsola yazdır
        $("#activecategories").text(value);
        animateCard("#activecategories");
    });
    connection.on("ReceivePassiveCategoryCount", value => {
        console.log("Received Passive Category Count:", value); // Konsola yazdır
        $("#passivecategories").text(value);
        animateCard("#passivecategories");
    });

    connection.on("ReceiveProductCount", value => {
        console.log("Received Product Count:", value); // Konsola yazdır
        $("#productcount").text(value);
        animateCard("#productcount");
    });
    connection.on("ReceiveLowestPricedProduct", value => {
        console.log("Received Lowest Priced Product:", value); // Konsola yazdır
        $("#lowestproduct").text(value);
        animateCard("#lowestproduct");
    });
    connection.on("ReceiveHighestPricedProduct", value => {
        console.log("Received Highest Priced Product:", value); // Konsola yazdır
        $("#highestproduct").text(value);
        animateCard("#highestproduct");
    });
    connection.on("ReceiveAvarageProductPrice", value => {
        console.log("Received Average Product Price:", value); // Konsola yazdır
        $("#averageproduct").text(value + " ₺");
        animateCard("#averageproduct");
    });
    connection.on("ReceiveAvarageHamburgerPrice", value => {
        console.log("Received Average Hamburger Price:", value); // Konsola yazdır
        $("#hamburgeraverage").text(value + " ₺");
        animateCard("#hamburgeraverage");
    });

    connection.on("ReceiveMoneyCase", value => {
        console.log("Received Money Case:", value); // Konsola yazdır
        $("#moneycase").text(value + " ₺");
        animateCard("#moneycase");
    });
    connection.on("ReceiveTodayTotalPrice", value => {
        console.log("Received Today Total Price:", value); // Konsola yazdır
        $("#todaytotal").text(value + " ₺");
        animateCard("#todaytotal");
    });

    connection.on("ReceiveOrderCount", value => {
        console.log("Received Order Count:", value); // Konsola yazdır
        $("#ordercount").text(value);
        animateCard("#ordercount");
    });
    connection.on("ReceiveActiveOrderCount", value => {
        console.log("Received Active Order Count:", value); // Konsola yazdır
        $("#activeorders").text(value);
        animateCard("#activeorders");
    });
    connection.on("ReceiveLastOrderPrice", value => {
        console.log("Received Last Order Price:", value); // Konsola yazdır
        $("#lastorder").text(value + " ₺");
        animateCard("#lastorder");
    });
    connection.on("ReceiveTotalTableCount", value => {
        console.log("Received Total Table Count:", value); // Konsola yazdır
        $("#totaltablecount").text(value);
        animateCard("#totaltablecount");
    });
    connection.on("ReceiveActiveTableCount", value => {
        console.log("Received Active Table Count:", value); // Konsola yazdır
        $("#activetable").text(value);
        animateCard("#activetable");
    });
    connection.on("ReceiveNotActiveTableCount", value => {
        console.log("Received Not Active Table Count:", value); // Konsola yazdır
        $("#notactivetable").text(value);
        animateCard("#notactivetable");
    });

    function animateCard(selector) {
        $(selector).closest('.card').addClass("pulse");
        setTimeout(() => {
            $(selector).closest('.card').removeClass("pulse");
        }, 500);
    }
});
