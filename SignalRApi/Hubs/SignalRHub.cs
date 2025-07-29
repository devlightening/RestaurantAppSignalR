using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstracts;
using SignalR.EntityLayer.Entities;

public class SignalRHub : Hub
{
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

    public async Task SendStatistic()
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
        await Clients.All.SendAsync("ReceiveActiveTableCount", availableTableCount); // İstemcideki ID'ye göre eşleşti

        var notAvailableTableCount = await _restaurantTableService.TNotAvailableTableCount();
        await Clients.All.SendAsync("ReceiveNotActiveTableCount", notAvailableTableCount); // İstemcideki ID'ye göre eşleşti
    }
    public async Task SendProgress()
    {

        var value = _moneyCaseService.TTotalMoneyCaseAmountAsync;
        await Clients.All.SendAsync("ReceiveMoneyCase", value);

        var value2 = _restaurantTableService.TNotAvailableTableCount();
        await Clients.All.SendAsync("ReceiveNotActiveTableCount", value2);


        var value3= _orderService.TActiveOrderNumber();
        await Clients.All.SendAsync("ReceiveActiveOrderCount", value3);

    }

    // Yeni eklenen method: client'tan toplam fiyatı al ve yayınla
    public async Task SendTotalPrice(decimal totalPrice)
    {
        await Clients.Caller.SendAsync("ReceiveTotalPrice", totalPrice);
    }
    public async Task SendNotifyBasketUpdated()
    {
        var value = _basketService.TotalBasketAmountAsync();
        await Clients.All.SendAsync("ReceiveBasketUpdate",value);
    }

    public async Task GetBookingList()
    {
        var value = _bookingService.TGetListAllAsync();
        await Clients.All.SendAsync("ReceiveGetBookingList", value);
    }
    public async Task SendNotification()
    {
        var value = _notificationService.TNotificationCountByStatusFalse();
        await Clients.All.SendAsync("ReceiveNotificationCountByFalse", value);

        var value1 = _notificationService.TGetAllNotificationByFalse();
        await Clients.All.SendAsync("ReceiveNotificationListByFalse", value1);
    }

    public async Task SendRestaurantTables()
    {
        var value1 = _restaurantTableService.TAvailableTableCount();
        await Clients.All.SendAsync("ReceiveRestaurantTableCountAvailable", value1);

        var value = _restaurantTableService.TGetAvailableTables();
        await Clients.All.SendAsync("ReceiveRestaurantTableByAvailable", value);
    }

    public async Task SendMessage(int senderUserId, int receiverUserId, string messageContent)
    {
        try
        {
            // 1. Gönderen kullanıcının bilgilerini veritabanından çek
            var senderUser = await _appUserService.TGetByIdAsync(senderUserId);
            if (senderUser == null)
            {
                await Clients.Caller.SendAsync("ReceiveError", $"Hata: Gönderen kullanıcı ID {senderUserId} bulunamadı. Lütfen geçerli bir ID girin.");
                Console.WriteLine($"Hata: Gönderen kullanıcı ID {senderUserId} bulunamadı. Mesaj gönderilmedi.");
                return;
            }

            // Gönderen kullanıcının tam adını oluştur
            string senderFullName = $"{senderUser.Name} {senderUser.Surname}";

            // 2. Alıcı kullanıcının bilgilerini veritabanından çek (genel sohbet değilse)
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
            var message = new Message
            {
                SenderUserId = senderUserId,
                ReceiverUserId = receiverUserId,
                Content = messageContent,
                Timestamp = DateTime.Now // Mesajın gönderildiği anın zaman damgası
            };

            await _messageService.TAddAsync(message); // Mesajı veritabanına asenkron olarak ekle

            // 4. Mesajı tüm bağlı istemcilere yayınla
            // İstemcilere gönderirken, tam adları ve zaman damgasını iletiyoruz.
            // Bu, chat.js'deki 'ReceiveMessage' dinleyicisi ile uyumlu olmalıdır (4 parametre).
            await Clients.All.SendAsync("ReceiveMessage", senderFullName, message.Content, message.Timestamp, receiverFullName);
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
