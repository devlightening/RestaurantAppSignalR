
    document.addEventListener("DOMContentLoaded", function () {
        const filterButtons = document.querySelectorAll('.filter-btn');
    const tablesGrid = document.getElementById('tablesGrid');
    const noTablesMessage = document.getElementById('noTablesMessage');
    const availableCountSpan = document.getElementById("availableTableCount");
    const toggleAvailable = document.getElementById("availableToggle");

    let currentFilter = 'all';
    let allTables = []; // Tüm masaları saklamak için

    // SignalR bağlantısını oluştur
    const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7000/signalrhub")
    .build();

        // SignalR bağlantısını başlat
        connection.start().then(() => {
        console.log("✅ SignalR bağlantısı kuruldu");
    loadInitialData();

            // Her 3 saniyede bir güncelleme istegi gönder
            setInterval(async () => {
                try {
        await connection.invoke("SendRestaurantTables");
                } catch (err) {
        console.error("SignalR invoke hatası:", err);
                }
            }, 3000);
        }).catch(err => {
        console.error("SignalR bağlantı hatası:", err);
    loadInitialData(); // SignalR bağlanamasa bile ilk verileri yükle
        });

        // SignalR'dan tüm masalar geldiğinde
        connection.on("ReceiveRestaurantTables", (tables) => {
        console.log("📥 SignalR - Tüm masalar güncellendi:", tables);
    allTables = tables;
    updateAvailableCount();
    applyCurrentFilter();
        });

        // SignalR'dan boş masa sayısı geldiğinde
        connection.on("ReceiveRestaurantTableCountAvailable", (count) => {
        console.log("📥 SignalR - Boş masa sayısı:", count);
    if (availableCountSpan) {
        availableCountSpan.textContent = `(${count})`;
            }
        });

    // İlk veri yükleme
    function loadInitialData() {
        fetch("https://localhost:7000/api/RestaurantTables")
            .then(res => res.json())
            .then(data => {
                allTables = data;
                updateAvailableCount();
                applyCurrentFilter();
            })
            .catch(err => {
                console.error("İlk veriler yüklenemedi:", err);
            });
        }

    // Boş masa sayısını güncelle
    function updateAvailableCount() {
            const availableCount = allTables.filter(table => !table.status).length;
    if (availableCountSpan) {
        availableCountSpan.textContent = `(${availableCount})`;
            }
        }

    // Mevcut filtreyi uygula
    function applyCurrentFilter() {
        let filteredTables = allTables;

    // Toggle aktif ise sadece boş masalar
    if (toggleAvailable.checked) {
        filteredTables = filteredTables.filter(table => !table.status);
            }

    // Konum filtresi
    if (currentFilter !== 'all') {
        filteredTables = filteredTables.filter(table =>
            table.location.toLowerCase() === currentFilter
        );
            }

    renderTableCards(filteredTables);
        }

    // Masa kartlarını oluştur
    function renderTableCards(tables) {
            if (!tables || tables.length === 0) {
        tablesGrid.style.display = "none";
    noTablesMessage.style.display = "block";

    if (toggleAvailable.checked) {
        noTablesMessage.querySelector('h3').textContent = "Boş Masa Bulunamadı";
    noTablesMessage.querySelector('p').textContent = "Şu anda seçilen kriterlere uygun boş masa bulunmamaktadır.";
                } else {
        noTablesMessage.querySelector('h3').textContent = "Masa Bulunamadı";
    noTablesMessage.querySelector('p').textContent = "Seçilen kriterlere uygun masa bulunmamaktadır.";
                }
    return;
            }

    noTablesMessage.style.display = "none";
    tablesGrid.style.display = "grid";

            const cardsHTML = tables.map(table => `
    <div class="table-card ${table.status ? 'occupied' : 'available'}"
        data-location="${table.location.toLowerCase()}"
        data-table-id="${table.restaurantTableId}"
        data-status="${table.status ? 'occupied' : 'available'}">
        <div class="table-info">
            <div class="table-icon">
                <i class="la la-cutlery"></i>
            </div>
            <h5 class="table-name">${table.location}</h5>
            <p class="table-id">Masa No: ${table.tableNo}</p>
            <span class="status-badge-grid ${table.status ? 'status-occupied' : 'status-available'}">
                <i class="la ${table.status ? 'la-user' : 'la-check'}"></i>
                ${table.status ? 'Dolu' : 'Boş'}
            </span>
        </div>
        <div class="table-actions">
            <a href="/RestaurantTable/UpdateRestaurantTable/${table.restaurantTableId}" class="btn-table-action btn-edit">
                <i class="la la-edit"></i> Güncelle
            </a>
            <form action="/RestaurantTable/DeleteRestaurantTable/${table.restaurantTableId}" method="post" onsubmit="return confirm('Bu masayı silmek istediğinize emin misiniz?');" style="display:inline; flex:1;">
                <button type="submit" class="btn-table-action btn-delete">
                    <i class="la la-trash"></i> Sil
                </button>
            </form>
        </div>
    </div>
    `).join('');

    tablesGrid.innerHTML = cardsHTML;

    // Animasyon ekle
    const cards = tablesGrid.querySelectorAll('.table-card');
            cards.forEach((card, index) => {
        card.style.opacity = '0';
    card.style.transform = 'translateY(20px)';
                setTimeout(() => {
        card.style.transition = 'all 0.4s ease';
    card.style.opacity = '1';
    card.style.transform = 'translateY(0)';
                }, index * 100);
            });
        }

        // Filtreleme butonları
        filterButtons.forEach(button => {
        button.addEventListener('click', function () {
            const filter = this.getAttribute('data-filter');
            currentFilter = filter;

            // Aktif buton stilini güncelle
            filterButtons.forEach(btn => btn.classList.remove('active'));
            this.classList.add('active');

            // Filtreyi uygula
            applyCurrentFilter();
        });
        });

    // Toggle değişikliği
    toggleAvailable.addEventListener("change", function () {
        console.log("🔄 Toggle durumu:", this.checked);
    applyCurrentFilter();
        });

    // Sayfa yüklendiğinde boş masa sayısını güncelle
    updateAvailableCount();
    });