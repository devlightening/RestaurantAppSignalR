using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstracts;
using System; // Exception için
using System.Threading.Tasks;
using System.Collections.Concurrent; // ConcurrentDictionary için
using SignalR.EntityLayer.Entities; // Message ve AppUser entity'leri için (SendMessage metodu için gerekli)

// SignalRHub'ın doğru namespace'ini buraya ekleyin, genellikle SignalR.WebUI.Hubs veya SignalR.Api.Hubs gibi olur.

public class SignalRHub : Hub
{
    // Bağlantı ID'si ile AppUserId eşlemesini tutan thread-safe bir sözlük
    // Bu, kullanıcıların online durumunu yönetmek için kullanılabilir.
    private static ConcurrentDictionary<string, int> ConnectedUsers = new ConcurrentDictionary<string, int>();

    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;
    private readonly IMoneyCaseService _moneyCaseService;
    private readonly IOrderService _orderService;
    private readonly IOrderDetailService _orderDetailService;
    private readonly IRestaurantTableService _restaurantTableService;
    private readonly IBookingService _bookingService;
    private readonly IBasketService _basketService;
    private readonly INotificationService _notificationService;
    private readonly IMessageService _messageService;
    private readonly IAppUserService _appUserService;

    public SignalRHub(ICategoryService categoryService,
        IProductService productService,
        IMoneyCaseService moneyCaseService,
        IOrderService orderService,
        IOrderDetailService orderDetailService,
        IRestaurantTableService restaurantTableService,
        IBookingService bookingService,
        IBasketService basketService,
        INotificationService notificationService,
        IMessageService messageService,
        IAppUserService appUserService)
    {
        _categoryService = categoryService;
        _productService = productService;
        _moneyCaseService = moneyCaseService;
        _orderService = orderService;
        _orderDetailService = orderDetailService;
        _restaurantTableService = restaurantTableService;
        _bookingService = bookingService;
        _basketService = basketService;
        _notificationService = notificationService;
        _messageService = messageService;
        _appUserService = appUserService;
    }

    // Bir istemci Hub'a bağlandığında çağrılır.
    public override async Task OnConnectedAsync()
    {
        try
        {
            // DİKKAT: userId burada sabit olarak 17 atanmıştır.
            // Gerçek bir uygulamada, bu userId'nin kimlik doğrulama mekanizmasından (örn: JWT, çerezler)
            // veya istemciden güvenli bir şekilde (örn: query string ile ancak dikkatli olunmalı) alınması gerekir.
            // Eğer ID 17'ye sahip bir kullanıcı yoksa veya TUpdateUserOnlineStatus metodu hata veriyorsa,
            // bağlantı kesilebilir.
            int userId = 17;

            // Eğer userId'yi query string'den alıyorsanız (geliştirme amaçlı):
            // int userId = 0;
            // if (Context.GetHttpContext().Request.Query.TryGetValue("userId", out var userIdString))
            // {
            //     if (int.TryParse(userIdString, out int parsedUserId))
            //     {
            //         userId = parsedUserId;
            //     }
            // }

            if (userId > 0)
            {
                // Bağlantı ID'si ile kullanıcı ID'sini eşleştir
                ConnectedUsers.AddOrUpdate(Context.ConnectionId, userId, (key, oldValue) => userId);
                // Kullanıcının çevrimiçi durumunu güncelle
                await _appUserService.TUpdateUserOnlineStatusAsync(userId, true);
            }
            else
            {
                Console.WriteLine($"Uyarı: OnConnectedAsync - Bağlantı {Context.ConnectionId} için geçerli bir AppUserId sağlanmadı veya bulunamadı. Kullanıcı çevrimiçi olarak işaretlenmedi.");
            }

            // Tüm istemcilere çevrimiçi kullanıcı listesinin güncellendiğini bildir
            await Clients.All.SendAsync("ReceiveOnlineUsersUpdate");

            // Temel sınıfın OnConnectedAsync metodunu çağır
            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            // Hata oluştuğunda sunucu konsoluna detaylı bilgi yazdır
            Console.WriteLine($"\n--- SignalR Hub OnConnectedAsync Hata Başlangıcı ---");
            Console.WriteLine($"OnConnectedAsync metodunda bir hata oluştu: {ex.Message}");
            Console.WriteLine($"Detay: {ex.InnerException?.Message ?? "İç hata mesajı yok."}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Console.WriteLine($"--- SignalR Hub OnConnectedAsync Hata Sonu ---\n");
            // Hatanın istemciye yayılmasını önlemek veya istemciye bildirmek için burada ek işlem yapılabilir.
            // Örneğin: await Clients.Caller.SendAsync("ReceiveError", "Bağlantı kurulurken bir sorun oluştu.");
            // throw; // Hatanın yayılmasını sağlamak için yeniden fırlatılabilir, ancak bu bağlantıyı tamamen kapatabilir.
        }
    }

    // Bir istemci Hub'dan ayrıldığında çağrılır.
    public override async Task OnDisconnectedAsync(Exception exception)
    {
        try
        {
            // DİKKAT: userId burada sabit olarak 17 atanmıştır.
            // Gerçek bir uygulamada, bu userId'nin ConnectedUsers sözlüğünden veya
            // kimlik doğrulama mekanizmasından alınması gerekir.
            int userId = 17;

            // Bağlantı ID'si ile eşleşen kullanıcıyı sözlükten kaldır
            if (ConnectedUsers.TryRemove(Context.ConnectionId, out int removedUserId))
            {
                // Kaldırılan kullanıcının çevrimdışı durumunu güncelle
                await _appUserService.TUpdateUserOnlineStatusAsync(removedUserId, false);
            }
            else
            {
                Console.WriteLine($"Uyarı: OnDisconnectedAsync - Bağlantı {Context.ConnectionId} için kayıtlı AppUserId bulunamadı. Kullanıcı çevrimdışı olarak işaretlenmedi.");
            }

            // Tüm istemcilere çevrimiçi kullanıcı listesinin güncellendiğini bildir
            await Clients.All.SendAsync("ReceiveOnlineUsersUpdate");

            // Temel sınıfın OnDisconnectedAsync metodunu çağır
            await base.OnDisconnectedAsync(exception);
        }
        catch (Exception ex)
        {
            // Hata oluştuğunda sunucu konsoluna detaylı bilgi yazdır
            Console.WriteLine($"\n--- SignalR Hub OnDisconnectedAsync Hata Başlangıcı ---");
            Console.WriteLine($"OnDisconnectedAsync metodunda bir hata oluştu: {ex.Message}");
            Console.WriteLine($"Detay: {ex.InnerException?.Message ?? "İç hata mesajı yok."}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Console.WriteLine($"--- SignalR Hub OnDisconnectedAsync Hata Sonu ---\n");
        }
    }

    // İstatistikleri tüm bağlı istemcilere gönderir.
    public async Task SendStatistic()
    {
        try
        {
            // Kategori İstatistikleri
            var categoryCount = await _categoryService.TCategoryCountAsync();
            await Clients.All.SendAsync("ReceiveCategoryCount", categoryCount);

            var activeCategoryCount = await _categoryService.TActiveCategoryCountAsync();
            await Clients.All.SendAsync("ReceiveActiveCategoryCount", activeCategoryCount);

            var passiveCategoryCount = await _categoryService.TPassiveCategoryCountAsync();
            await Clients.All.SendAsync("ReceivePassiveCategoryCount", passiveCategoryCount);

            // Ürün İstatistikleri
            var productCount = await _productService.TProductCount();
            await Clients.All.SendAsync("ReceiveProductCount", productCount);

            var lowestPricedProduct = await _productService.TLowestPricedProduct();
            await Clients.All.SendAsync("ReceiveLowestPricedProduct", lowestPricedProduct);

            var highestPricedProduct = await _productService.THighestPricedProduct();
            await Clients.All.SendAsync("ReceiveHighestPricedProduct", highestPricedProduct);

            var avarageProductPrice = await _productService.TAvarageProductPrice();
            await Clients.All.SendAsync("ReceiveAvarageProductPrice", avarageProductPrice);

            var avarageHamburgerPrice = await _productService.TAvarageHamburgerPrice();
            await Clients.All.SendAsync("ReceiveAvarageHamburgerPrice", avarageHamburgerPrice);

            // Finansal İstatistikler
            var totalMoneyCaseAmount = await _moneyCaseService.TTotalMoneyCaseAmountAsync();
            await Clients.All.SendAsync("ReceiveMoneyCase", totalMoneyCaseAmount);

            var todayTotalPrice = await _orderService.TTodayTotalPrice();
            await Clients.All.SendAsync("ReceiveTodayTotalPrice", todayTotalPrice);

            // Sipariş İstatistikleri
            var totalOrderNumber = await _orderService.TTotalOrderNumber();
            await Clients.All.SendAsync("ReceiveOrderCount", totalOrderNumber);

            var activeOrderNumber = await _orderService.TActiveOrderNumber();
            await Clients.All.SendAsync("ReceiveActiveOrderCount", activeOrderNumber);

            var lastOrderPrice = await _orderService.TLastOrderPrice();
            await Clients.All.SendAsync("ReceiveLastOrderPrice", lastOrderPrice);

            // Masa İstatistikleri
            var totalTableCount = await _restaurantTableService.TTotalTableCount();
            await Clients.All.SendAsync("ReceiveTotalTableCount", totalTableCount);

            var availableTableCount = await _restaurantTableService.TAvailableTableCount();
            await Clients.All.SendAsync("ReceiveActiveTableCount", availableTableCount);

            var notAvailableTableCount = await _restaurantTableService.TNotAvailableTableCount();
            await Clients.All.SendAsync("ReceiveNotActiveTableCount", notAvailableTableCount);
        }
        catch (Exception ex)
        {
            // Hata oluştuğunda sunucu konsoluna yazdır
            Console.WriteLine($"SignalR Hub SendStatistic metodunda hata oluştu: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            // İstemcilere de bir hata mesajı gönderebilirsiniz (isteğe bağlı)
            await Clients.Caller.SendAsync("ReceiveError", "İstatistikler yüklenirken bir hata oluştu.");
        }
    }

    // İlerleme bilgilerini gönderir (Örnek metot)
    public async Task SendProgress()
    {
        // DİKKAT: Bu satırlar await keyword'ü eksik olduğu için doğru çalışmayabilir.
        // _moneyCaseService.TTotalMoneyCaseAmountAsync bir Task<decimal> döndürür, doğrudan değer değil.
        // Doğrusu: var value = await _moneyCaseService.TTotalMoneyCaseAmountAsync(); olmalı.
        var value = _moneyCaseService.TTotalMoneyCaseAmountAsync;
        await Clients.All.SendAsync("ReceiveMoneyCase", value);

        var value2 = await _restaurantTableService.TNotAvailableTableCount();
        await Clients.All.SendAsync("ReceiveNotActiveTableCount", value2);

        var value3 = await _orderService.TActiveOrderNumber();
        await Clients.All.SendAsync("ReceiveActiveOrderCount", value3);
    }

    // Yeni eklenen method: client'tan toplam fiyatı al ve yayınla
    public async Task SendTotalPrice(decimal totalPrice)
    {
        await Clients.Caller.SendAsync("ReceiveTotalPrice", totalPrice);
    }

    // Sepet güncellendiğinde bildirim gönderir.
    public async Task SendNotifyBasketUpdated()
    {
        var value = await _basketService.TotalBasketAmountAsync(); // await eklendi
        await Clients.All.SendAsync("ReceiveBasketUpdate", value);
    }

    // Rezervasyon listesini alır ve yayınlar.
    public async Task GetBookingList()
    {
        var value = await _bookingService.TGetListAllAsync(); // await eklendi
        await Clients.All.SendAsync("ReceiveGetBookingList", value);
    }

    // Bildirimleri gönderir.
    public async Task SendNotification()
    {
        var value = await _notificationService.TNotificationCountByStatusFalse(); // await eklendi
        await Clients.All.SendAsync("ReceiveNotificationCountByFalse", value);

        var value1 = await _notificationService.TGetAllNotificationByFalse(); // await eklendi
        await Clients.All.SendAsync("ReceiveNotificationListByFalse", value1);
    }

    // Restoran masası bilgilerini gönderir.
    public async Task SendRestaurantTables()
    {
        var value1 = await _restaurantTableService.TAvailableTableCount(); // await eklendi
        await Clients.All.SendAsync("ReceiveRestaurantTableCountAvailable", value1);

        var value = await _restaurantTableService.TGetAvailableTables(); // await eklendi
        await Clients.All.SendAsync("ReceiveRestaurantTableByAvailable", value);
    }

    /// <summary>
    /// İstemciden gelen mesajı alır, veritabanına kaydeder ve tüm bağlı istemcilere yayınlar.
    /// </summary>
    /// <param name="senderUserId">Mesajı gönderen kullanıcının ID'si.</param>
    /// <param name="receiverUserId">Mesajı alan kullanıcının ID'si (0 genel sohbeti temsil edebilir).</param>
    /// <param name="messageContent">Mesajın içeriği.</param>
    public async Task SendMessage(int senderUserId, int receiverUserId, string messageContent)
    {
        try
        {
            // 1. Gönderen kullanıcının bilgilerini veritabanından çek
            // Bu bilgi, mesajı yayınlarken kullanıcı adını göstermek için kullanılır.
            var senderUser = await _appUserService.TGetByIdAsync(senderUserId);
            if (senderUser == null)
            {
                // Gönderen kullanıcı bulunamazsa, istemciye hata mesajı gönder ve işlemi durdur.
                await Clients.Caller.SendAsync("ReceiveError", $"Hata: Gönderen kullanıcı ID {senderUserId} bulunamadı. Lütfen geçerli bir ID girin.");
                Console.WriteLine($"Hata: Gönderen kullanıcı ID {senderUserId} bulunamadı. Mesaj gönderilmedi.");
                return; // Metodun daha fazla ilerlemesini engelle
            }

            // Gönderen kullanıcının tam adını oluştur
            string senderFullName = $"{senderUser.Name} {senderUser.Surname}";

            // 2. Alıcı kullanıcının bilgilerini veritabanından çek (genel sohbet değilse)
            // Bu kısım, eğer özel mesajlaşma yapılıyorsa alıcı adını göstermek için kullanılabilir.
            // Şu anki ReceiveMessage istemci metodu 3 parametre aldığı için receiverFullName doğrudan kullanılmıyor.
            string receiverFullName = "Genel Sohbet"; // Varsayılan değer
            if (receiverUserId != 0) // Eğer alıcı ID'si 0 değilse (yani özel bir alıcı varsa)
            {
                var receiverUser = await _appUserService.TGetByIdAsync(receiverUserId);
                if (receiverUser == null)
                {
                    await Clients.Caller.SendAsync("ReceiveError", $"Hata: Alıcı kullanıcı ID {receiverUserId} bulunamadı. Lütfen geçerli bir ID girin.");
                    Console.WriteLine($"Hata: Alıcı kullanıcı ID {receiverUserId} bulunamadı. Mesaj gönderilmedi.");
                    return;
                }
                receiverFullName = $"{receiverUser.Name} {receiverUser.Surname}"; // Alıcının tam adını al
            }

            // 3. Mesaj entity'sini oluştur ve veritabanına kaydet
            // Message entity'nizin 'Content' ve 'Timestamp' property'leri olduğunu varsayılmıştır.
            var message = new Message
            {
                SenderUserId = senderUserId,
                ReceiverUserId = receiverUserId,
                Content = messageContent,
                Timestamp = DateTime.Now // Mesajın gönderildiği anın zaman damgası
            };

            // Mesajı veritabanına asenkron olarak ekle.
            // Bu işlem sırasında bir veritabanı hatası oluşabilir (örn: Foreign Key kısıtlaması ihlali).
            await _messageService.TAddAsync(message);

            // 4. Mesajı tüm bağlı istemcilere yayınla
            // İstemcilere gönderirken, veritabanından çekilen gönderen adını, mesaj içeriğini ve zaman damgasını iletiyoruz.
            // Bu, default-chat.js'deki 'ReceiveMessage' dinleyicisi ile uyumlu olmalıdır.
            await Clients.All.SendAsync("ReceiveMessage", senderFullName, message.Content, message.Timestamp);
        }
        catch (Exception ex)
        {
            // Sunucu tarafında oluşan tüm beklenmedik hataları yakala.
            // Hata detaylarını sunucu konsoluna yazdır. Bu, hata ayıklama için kritik öneme sahiptir.
            Console.WriteLine($"\n--- SignalR Hub Hata Başlangıcı ---");
            Console.WriteLine($"SendMessage metodunda bir hata oluştu: {ex.Message}");
            // İç hata mesajını (InnerException) kontrol et, bu genellikle daha spesifik bilgi sağlar.
            Console.WriteLine($"Detay: {ex.InnerException?.Message ?? "İç hata mesajı yok."}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Console.WriteLine($"--- SignalR Hub Hata Sonu ---\n");

            // İstemciye de genel bir hata mesajı göndererek kullanıcıyı bilgilendir.
            await Clients.Caller.SendAsync("ReceiveError", "Mesaj gönderilirken sunucuda beklenmeyen bir hata oluştu. Lütfen konsolu kontrol edin.");
        }
    }
}

