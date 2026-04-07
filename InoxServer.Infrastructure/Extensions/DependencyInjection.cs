using InoxServer.Domain.Configuration;
using InoxServer.Domain.Interfaces.Services;
using InoxServer.Domain.Interfaces.Repositories;
using InoxServer.Infrastructure.Contexts;
using InoxServer.Infrastructure.Repositories;
using InoxServer.Infrastructure.Services.Auth;
using InoxServer.Infrastructure.Services.Cloudinary;
using InoxServer.Infrastructure.Services.EmailService;
using InoxServer.Infrastructure.Services.PayOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace InoxServer.Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PayOsOptions>(configuration.GetSection(PayOsOptions.SectionName));
            services.Configure<AppOptions>(configuration.GetSection(AppOptions.SectionName));

            services.AddHttpClient<IPayOsPaymentClient, PayOsPaymentClient>((sp, client) =>
            {
                var opts = sp.GetRequiredService<IOptions<PayOsOptions>>().Value;
                var baseUrl = string.IsNullOrWhiteSpace(opts.BaseUrl)
                    ? "https://api-merchant.payos.vn"
                    : opts.BaseUrl.TrimEnd('/');
                client.BaseAddress = new Uri($"{baseUrl}/");
                if (!string.IsNullOrWhiteSpace(opts.ClientId))
                    client.DefaultRequestHeaders.TryAddWithoutValidation("x-client-id", opts.ClientId);
                if (!string.IsNullOrWhiteSpace(opts.ApiKey))
                    client.DefaultRequestHeaders.TryAddWithoutValidation("x-api-key", opts.ApiKey);
            });
            services.AddSingleton<IPayOsWebhookSignatureVerifier, PayOsWebhookSignatureVerifier>();

            // DbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // =========================
            // Repositories
            // =========================
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICartRepository, CartRepository>();
            services.AddScoped<ICartItemRepository, CartItemRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IWishlistRepository, WishlistRepository>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICouponUsageRepository, CouponUsageRepository>();
            services.AddScoped<IBannerRepository, BannerRepository>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ICloudinaryService, CloudinaryService>();

            // =========================
            // Unit Of Work
            // =========================
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}