using SignalR.BusinessLayer.Abstracts;
using SignalR.BusinessLayer.Concretes;
using SignalR.DataAccessLayer.Abstracts;
using SignalR.DataAccessLayer.Concrete;
using SignalR.DataAccessLayer.EntityFramework;
using System.Reflection;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // CORS politikasý ekle
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyHeader()
                       .AllowAnyMethod()
                       .SetIsOriginAllowed((host) => true) // Geliþtirme için tüm kaynaklara izin ver
                       .AllowCredentials(); // Kimlik bilgileriyle istek göndermeye izin ver
            });
        });

        // SignalR servisini ekle
        builder.Services.AddSignalR();

        // DbContext'i bir kez ekle
        builder.Services.AddDbContext<SignalRContext>();

        // AutoMapper'ý ekle
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Servis ve DAL baðýmlýlýklarýný ekle (Mevcut haliyle býrakýldý, doðru görünüyor)
        builder.Services.AddScoped<IAboutService, AboutManager>();
        builder.Services.AddScoped<IAboutDal, EFAboutDal>();

        builder.Services.AddScoped<IBookingService, BookingManager>();
        builder.Services.AddScoped<IBookingDal, EfBookingDal>();

        builder.Services.AddScoped<ICategoryService, CategoryManager>();
        builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

        builder.Services.AddScoped<IContactService, ContactManager>();
        builder.Services.AddScoped<IContactDal, EfContactDal>();

        builder.Services.AddScoped<IDiscountService, DiscountManager>();
        builder.Services.AddScoped<IDiscountDal, EfDiscountDal>();

        builder.Services.AddScoped<IFeatureService, FeatureManager>();
        builder.Services.AddScoped<IFeatureDal, EfFeatureDal>();

        builder.Services.AddScoped<IProductService, ProductManager>();
        builder.Services.AddScoped<IProductDal, EfProductDal>();

        builder.Services.AddScoped<ISocialMediaService, SocialMediaManager>();
        builder.Services.AddScoped<ISocialMediaDal, EfSocialMediaDal>();

        builder.Services.AddScoped<ITestimonialService, TestimonialManager>();
        builder.Services.AddScoped<ITestimonialDal, EfTestimonialDal>();

        builder.Services.AddScoped<IOrderDetailService, OrderDetailManager>();
        builder.Services.AddScoped<IOrderDetailDal, EfOrderDetailDal>();

        builder.Services.AddScoped<IOrderService, OrderManager>();
        builder.Services.AddScoped<IOrderDal, EfOrderDal>();

        builder.Services.AddScoped<IMoneyCaseService, MoneyCaseManager>();
        builder.Services.AddScoped<IMoneyCaseDal, EfMoneyCaseDal>();

        builder.Services.AddScoped<IRestaurantTableService, RestaurantTableManager>();
        builder.Services.AddScoped<IRestaurantTableDal, EfRestaurantTable>();

        builder.Services.AddScoped<ISliderService, SliderManager>();
        builder.Services.AddScoped<ISliderDal, EfSliderDal>();

        builder.Services.AddScoped<IBasketService, BasketManager>();
        builder.Services.AddScoped<IBasketDal, EfBasketDal>();

        builder.Services.AddScoped<INotificationService, NotificationManager>();
        builder.Services.AddScoped<INotificationDal, EfNotificationDal>();

        builder.Services.AddScoped<IAppUserService, AppUserManager>();
        builder.Services.AddScoped<IAppUserDal, EfAppUserDal>();

        builder.Services.AddScoped<IMessageService, MessageManager>();
        builder.Services.AddScoped<IMessageDal, EfMessageDal>();

        // MVC ve API Controller'larý için tek bir AddControllersWithViews çaðrýsý
        builder.Services.AddControllersWithViews()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

        // Swagger/OpenAPI
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // HTTP istek hattýný yapýlandýr.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // CORS'u etkinleþtir (UseRouting'den önce gelmeli)
        app.UseCors("CorsPolicy");

        // HTTPS yönlendirmesi
        app.UseHttpsRedirection();

        // Statik dosyalarý kullan (CSS, JS, resimler vb.)
        app.UseStaticFiles(); // Bu satýr genellikle otomatik olarak eklenir, ancak emin olmak için eklenebilir.

        // Kimlik doðrulama ve yetkilendirme
        app.UseAuthentication(); // Eðer kimlik doðrulama kullanýyorsanýz
        app.UseAuthorization();

        // Routing middleware'i (MapControllers ve MapHub'dan önce gelir)
        app.UseRouting(); // Genellikle MapControllers/MapHub tarafýndan örtük olarak çaðrýlýr, ancak açýkça belirtmek sorun çözebilir.

        // Controller eþlemesi
        app.MapControllers();

        // SignalR Hub eþlemesi
        app.MapHub<SignalRHub>("/signalrhub"); // Hub URL'si

        app.Run();
    }
}
