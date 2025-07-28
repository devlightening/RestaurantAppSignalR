$(document).ready(() => {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7000/signalrhub")
        .build();

    connection.start().then(() => {
        $("#connstatus").text("Bağlandı").removeClass("pulse");

        setInterval(async () => {
            try {
                await connection.invoke("GetBookingList");
            } catch (err) {
                console.error("GetBookingList error:", err);
                $("#connstatus").addClass("pulse").text("Hata");
            }
        }, 1000);
    }).catch(err => {
        console.error("SignalR bağlantı hatası:", err);
        $("#connstatus").text("Bağlantı Hatası").addClass("pulse");
    });

    connection.on("ReceiveGetBookingList", data => {
        const tbody = $("#booking-table-body");
        tbody.empty();

        if (data && data.length > 0) {
            $(".empty-state").hide();
            $(".table-responsive").show();

            let count = 0;
            data.forEach(item => {
                count++;
                tbody.append(`
                    <tr>
                        <td><strong>${count}</strong></td>
                        <td>${item.name}</td>
                        <td>${item.phone}</td>
                        <td>${item.mail}</td>
                        <td><strong>${item.personCount}</strong></td>
                        <td>${new Date(item.date).toLocaleDateString()}</td>
                        <td><span class="status-badge">Rezervasyon Alındı</span></td>
                        <td>
                            <a href="/Booking/UpdateBooking/${item.bookingId}" class="btn-action btn-edit">
                                <i class="la la-edit"></i>
                            </a>
                            <a href="/Booking/DeleteBooking/${item.bookingId}" class="btn-action btn-delete" onclick="return confirm('Silmek istediğinize emin misiniz?')">
                                <i class="la la-trash"></i>
                            </a>
                        </td>
                    </tr>
                `);
            });
        } else {
            $(".table-responsive").hide();
            $(".empty-state").show();
        }
    });
});
