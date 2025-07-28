let couponApplied = false;

document.addEventListener('DOMContentLoaded', () => {
    /* 📦 ADET ARTTIR / AZALT */
    document.querySelectorAll('.increment').forEach(btn => {
        btn.addEventListener('click', () => updateQuantity(btn.dataset.id, +1));
    });

    document.querySelectorAll('.decrement').forEach(btn => {
        btn.addEventListener('click', () => updateQuantity(btn.dataset.id, -1));
    });

    async function updateQuantity(basketId, change) {
        const quantitySpan = document.getElementById(`qty-${basketId}`);
        let currentQty = parseInt(quantitySpan.innerText);
        const newQty = currentQty + change;
        if (newQty < 1) return;

        const response = await fetch(`https://localhost:7000/api/Baskets/${basketId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ count: newQty })
        });

        if (response.ok) {
            quantitySpan.innerText = newQty;

            const row = quantitySpan.closest('tr');
            const unitPrice = parseFloat(row.children[3].innerText.replace(',', '.'));
            const newLineTotal = +(unitPrice * newQty).toFixed(2);
            row.children[4].innerText = newLineTotal.toFixed(2) + ' ₺';

            updateSummary();

            try {
                await connection.invoke("SendNotifyBasketUpdated");
            } catch (err) {
                console.error("SignalR invoke error:", err);
            }
        } else {
            Swal.fire({
                icon: 'error',
                title: 'Hata',
                text: 'Adet güncellenemedi'
            });
        }
    }

    /* 🗑 ÜRÜN SİLME */
    document.querySelectorAll('.btn-delete').forEach(btn => {
        btn.addEventListener('click', async () => {
            const id = btn.dataset.id;
            const name = btn.dataset.name;

            const res = await Swal.fire({
                title: `${name} ürünü silinsin mi?`,
                icon: 'warning',
                showCancelButton: true,
                confirmButtonText: 'Evet',
                cancelButtonText: 'Hayır'
            });

            if (!res.isConfirmed) return;

            const del = await fetch(`https://localhost:7000/api/Baskets/${id}`, { method: 'DELETE' });

            if (del.ok) {
                btn.closest('tr').remove();
                updateSummary();

                Swal.fire({
                    icon: 'success',
                    title: 'Silindi',
                    text: `${name} sepetten kaldırıldı`,
                    timer: 1500,
                    showConfirmButton: false
                });
            } else {
                Swal.fire({
                    icon: 'error',
                    title: 'Hata',
                    text: 'Silme işlemi başarısız oldu'
                });
            }
        });
    });

    /* 🧮 SEPET ÖZETİ GÜNCELLE */
    window.updateSummary = function () {
        let sub = 0;
        document.querySelectorAll('tbody tr').forEach(r => {
            const line = parseFloat(r.children[4].innerText.replace('₺', '').trim().replace(',', '.'));
            if (!isNaN(line)) sub += line;
        });

        if (couponApplied) sub *= 0.85;

        const tip = +(sub * 0.04).toFixed(2);
        const total = +(sub + tip).toFixed(2);

        document.getElementById('subTotal').innerText = sub.toFixed(2) + ' ₺';
        document.getElementById('tip').innerText = tip.toFixed(2) + ' ₺';
        document.getElementById('grandTotal').innerText = total.toFixed(2) + ' ₺';
    };

    /* 🎟️ KUPON UYGULAMA */
    document.getElementById('applyCoupon')?.addEventListener('click', function () {
        if (couponApplied) {
            Swal.fire({
                icon: 'warning',
                title: 'Kupon Zaten Kullanıldı',
                text: 'Bu kupon zaten uygulanmış durumda'
            });
            return;
        }

        const code = document.getElementById('couponCode').value.trim().toUpperCase();

        if (!code) {
            Swal.fire({
                icon: 'info',
                title: 'Kupon Kodu Girmediniz',
                text: 'Lütfen bir kupon kodu girin'
            });
            return;
        }

        if (code !== 'AHSEN15') {
            Swal.fire({
                icon: 'error',
                title: 'Geçersiz Kupon',
                text: 'Lütfen geçerli bir kupon kodu girin'
            });
            return;
        }

        couponApplied = true;
        this.classList.add('applied');
        this.innerHTML = '<i class="fa fa-check"></i> Uygulandı';
        this.disabled = true;
        document.getElementById('couponCode').disabled = true;
        document.getElementById('couponSuccess').style.display = 'block';

        updateSummary();

        Swal.fire({
            icon: 'success',
            title: 'Kupon Uygulandı!',
            text: '%15 indirim kazandınız',
            timer: 2000,
            showConfirmButton: false
        });
    });

    /* 🔄 SIGNALR ENTEGRASYONU */
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7000/signalrhub")
        .build();

    connection.start().then(() => {
        $("#connstatus").text("Bağlandı").removeClass("pulse");

        setInterval(async () => {
            try {
                await connection.invoke("SendNotifyBasketUpdated");
            } catch (err) {
                console.error("SignalR invoke error:", err);
                $("#connstatus").addClass("pulse").text("Hata");
            }
        }, 1000);
    }).catch(err => {
        console.error("SignalR bağlantı hatası:", err);
        $("#connstatus").text("Bağlantı Hatası").addClass("pulse");
    });

    connection.on("NotifyBasketUpdated", (data) => {
        console.log("Sepet güncellendi:", data);
        updateSummary();
    });
});
