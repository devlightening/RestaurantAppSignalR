
$(document).ready(() => {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7000/signalrhub")
        .build();

    $("#connstatus").text(connection.state);

    connection.start().then(() => {
        $("#connstatus").text(connection.state);

        // Bağlantı başarılı olduğunda pulse efektini kaldır
        $("#connstatus").removeClass("pulse");

        setInterval(async () => {
            try {
                await connection.invoke("SendStatistic");
            } catch (err) {
                console.error("SignalR error:", err);
                $("#connstatus").addClass("pulse");
            }
        }, 1000);
    }).catch(err => {
        console.error("SignalR connection error:", err);
        $("#connstatus").text("Bağlantı Hatası");
        $("#connstatus").addClass("pulse");
    });

    // SignalR event handlers with animation
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


    // Animasyon fonksiyonu
    function animateCard(selector) {
        $(selector).closest('.card').addClass("pulse");
        setTimeout(() => {
            $(selector).closest('.card').removeClass("pulse");
        }, 500);
    }
});
