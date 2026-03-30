using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace InoxServer.Presentation.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddJwtAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var secretKey = jwtSettings["SecretKey"]
                ?? throw new Exception("Jwt:SecretKey is missing in configuration.");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(secretKey))
                    };
                    //xoa 11 dong dưới

                    // Token sai/hết hạn (vd. Swagger "Authorize" còn Bearer cũ) có thể làm handler ném exception → 500.
                    // Gán null để không rethrow; endpoint [Authorize] vẫn trả 401 khi chưa đăng nhập hợp lệ.
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            context.Exception = null!; // Ngăn rethrow; property không nullable trong API nhưng null được dùng để suppress.
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();

            return services;
        }
    }
}