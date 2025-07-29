using Microsoft.AspNetCore.SignalR;
using SignalR.BusinessLayer.Abstracts;

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

    public SignalRHub(ICategoryService categoryService, 
        IProductService productService,
        IMoneyCaseService moneyCaseService, 
        IOrderService orderService,
        IOrderDetailService orderDetailService, 
        IRestaurantTableService restaurantTableService, 
        IBookingService bookingService,
        IBasketService basketService,
        INotificationService notificationService)

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

    public async Task SendMessage(string user,string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message); 
    }
}
