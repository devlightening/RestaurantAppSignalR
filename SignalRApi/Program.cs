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

        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .SetIsOriginAllowed((host) => true)
                           .AllowCredentials();
            });

        });
        builder.Services.AddSignalR();
        builder.Services.AddDbContext<SignalRContext>(); // EF Core ba�lam�

        builder.Services.AddDbContext<SignalRContext>();
        builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

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

        builder.Services.AddControllersWithViews()
        .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


        builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        app.UseCors("CorsPolicy");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<SignalRHub>("/signalrhub");

        app.Run();
    }
}