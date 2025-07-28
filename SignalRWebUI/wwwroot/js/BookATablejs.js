async function submitForm() {
    const name = document.getElementById('name').value.trim();
    const phone = document.getElementById('phone').value.trim();
    const mail = document.getElementById('mail').value.trim();
    const personCount = document.getElementById('personCount').value;
    const reservationDate = document.getElementById('reservationDate').value;

    if (!name || !phone || !mail || personCount == 0 || !reservationDate) {
        Swal.fire('Eksik Bilgi', 'Lütfen tüm alanları doldurun.', 'warning');
        return;
    }

    const data = {
        Name: name,
        Phone: phone,
        Mail: mail,
        PersonCount: personCount,
        ReservationDate: reservationDate,
        Status: true
    };

    try {
        const response = await fetch('https://localhost:7000/api/Bookings', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        if (response.ok) {
            Swal.fire('Başarılı', 'Rezervasyon başarıyla oluşturuldu!', 'success');

            const formattedDate = new Date(reservationDate).toLocaleString('tr-TR', {
                year: 'numeric', month: 'long', day: 'numeric',
                hour: '2-digit', minute: '2-digit'
            });

            document.getElementById('successDateText').textContent = formattedDate;
            document.getElementById('reservationSuccessCard').classList.remove('d-none');

            document.getElementById('bookingForm').reset();
        } else {
            Swal.fire('Hata', 'Bir hata oluştu. Lütfen tekrar deneyin.', 'error');
        }
    } catch (error) {
        console.error('Request error', error);
        Swal.fire('Hata', 'Bir hata oluştu. Lütfen tekrar deneyin.', 'error');
    }
}