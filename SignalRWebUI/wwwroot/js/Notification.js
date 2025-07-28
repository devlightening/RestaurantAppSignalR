// 🔔 Bildirim "Okundu" olarak işaretle
$(document).ready(function () {
    $(".mark-read-btn").click(function () {
        const id = $(this).data("id");         // Bildirim ID'si
        const $btn = $(this);                  // Tıklanan buton
        const $row = $btn.closest("tr");       // İlgili tablo satırı

        $.ajax({
            url: `https://localhost:7000/api/Notifications/NotificationStatusTrue/${id}`,
            type: "GET",

            success: function () {
                // 🟢 Durum hücresini güncelle
                const $statusCell = $row.find(".badge-status");
                $statusCell
                    .removeClass("false")
                    .addClass("true")
                    .text("Okundu");

                // 🟢 Butonu kaldır, yerine onay işareti ekle
                $btn.closest("td").html('<span class="text-success fw-bold">✔ Okundu</span>');
            },

            error: function () {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata',
                    text: 'Bildirimi okundu yaparken bir sorun oluştu.'
                });
            }
        });
    });
});
