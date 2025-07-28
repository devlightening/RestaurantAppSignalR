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
        var value = _categoryService.TCategoryCount();
        await Clients.All.SendAsync("ReceiveCategoryCount", value);

        var value2 = _productService.TProductCount();
        await Clients.All.SendAsync("ReceiveProductCount", value2);

        var value3 = _moneyCaseService.TTotalMoneyCaseAmount();
        await Clients.All.SendAsync("ReceiveMoneyCase", value3);

        var value4 = _orderService.TTotalOrderNumber();
        await Clients.All.SendAsync("ReceiveOrderCount", value4);

        var value5 = _categoryService.TActiveCategoryCount();
        await Clients.All.SendAsync("ReceiveActiveCategoryCount", value5);

        var value6 = _categoryService.TPassiveCategoryCount();
        await Clients.All.SendAsync("ReceivePassiveCategoryCount", value6);

        var value7 = _orderService.TActiveOrderNumber();
        await Clients.All.SendAsync("ReceiveActiveOrderCount", value7);

        var value8 = _orderService.TLastOrderPrice();
        await Clients.All.SendAsync("ReceiveLastOrderPrice", value8);

        var value9 = _orderService.TTodayTotalPrice();
        await Clients.All.SendAsync("ReceiveTodayTotalPrice", value9);

        var value10 = _productService.TAvarageProductPrice();
        await Clients.All.SendAsync("ReceiveAvarageProductPrice", value10);

        var value11 = _productService.TLowestPricedProduct();
        await Clients.All.SendAsync("ReceiveLowestPricedProduct", value11);

        var value12 = _productService.THighestPricedProduct();
        await Clients.All.SendAsync("ReceiveHighestPricedProduct", value12);

        var value13 = _productService.TAvarageHamburgerPrice();
        await Clients.All.SendAsync("ReceiveAvarageHamburgerPrice", value13);

        var value14 = _restaurantTableService.TTotalTableCount();
        await Clients.All.SendAsync("ReceiveTotalTableCount", value14);

        var value15 = _restaurantTableService.TAvailableTableCount();
        await Clients.All.SendAsync("ReceiveActiveTableCount", value15);

        var value16 = _restaurantTableService.TNotAvailableTableCount();
        await Clients.All.SendAsync("ReceiveNotActiveTableCount", value16);


    }

    public async Task SendProgress()
    {

        var value = _moneyCaseService.TTotalMoneyCaseAmount();
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
        var value = _basketService.TBasketCount();
        await Clients.All.SendAsync("ReceiveBasketUpdate",value);
    }

    public async Task GetBookingList()
    {
        var value = _bookingService.TGetListAll();
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
