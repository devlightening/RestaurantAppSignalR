$(document).ready(() => {
    let connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7000/signalrhub")
        .withAutomaticReconnect({
            nextRetryDelayInMilliseconds: retryContext => {
                if (retryContext.elapsedMilliseconds < 60000) {
                    // Retry after 0, 2, 10, and 30 seconds
                    return [0, 2000, 10000, 30000][retryContext.previousRetryCount];
                }
                // Retry after 30 seconds for all further attempts
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
                    console.error("SignalR invoke error:", err);
                    $("#connstatus").addClass("pulse");
                }
            }, 1000);

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
        startConnection();
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

    connection.on("ReceiveCategoryCount", value => {
        $("#categorycount").text(value);
        animateCard("#categorycount");
    });
    connection.on("ReceiveActiveCategoryCount", value => {
        $("#activecategories").text(value);
        animateCard("#activecategories");
    });
    connection.on("ReceivePassiveCategoryCount", value => {
        $("#passivecategories").text(value);
        animateCard("#passivecategories");
    });

    connection.on("ReceiveProductCount", value => {
        $("#productcount").text(value);
        animateCard("#productcount");
    });
    connection.on("ReceiveLowestPricedProduct", value => {
        $("#lowestproduct").text(value);
        animateCard("#lowestproduct");
    });
    connection.on("ReceiveHighestPricedProduct", value => {
        $("#highestproduct").text(value);
        animateCard("#highestproduct");
    });
    connection.on("ReceiveAvarageProductPrice", value => {
        $("#averageproduct").text(value + " ₺");
        animateCard("#averageproduct");
    });
    connection.on("ReceiveAvarageHamburgerPrice", value => {
        $("#hamburgeraverage").text(value + " ₺");
        animateCard("#hamburgeraverage");
    });

    connection.on("ReceiveMoneyCase", value => {
        $("#moneycase").text(value + " ₺");
        animateCard("#moneycase");
    });
    connection.on("ReceiveTodayTotalPrice", value => {
        $("#todaytotal").text(value + " ₺");
        animateCard("#todaytotal");
    });

    connection.on("ReceiveOrderCount", value => {
        $("#ordercount").text(value);
        animateCard("#ordercount");
    });
    connection.on("ReceiveActiveOrderCount", value => {
        $("#activeorders").text(value);
        animateCard("#activeorders");
    });
    connection.on("ReceiveLastOrderPrice", value => {
        $("#lastorder").text(value + " ₺");
        animateCard("#lastorder");
    });
    connection.on("ReceiveTotalTableCount", value => {
        $("#totaltablecount").text(value);
        animateCard("#totaltablecount");
    });
    connection.on("ReceiveActiveTableCount", value => {
        $("#activetable").text(value);
        animateCard("#activetable");
    });
    connection.on("ReceiveNotActiveTableCount", value => {
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
