using WebApplication1.Models;

namespace WebApplication1.Extensions
{
    public static class AppConfigExtensions
    {
        public static WebApplicationBuilder ConfigureCors(this WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyMethod()
                                      .AllowAnyHeader());
            });
            return builder;
        }


        public static IServiceCollection AddAppConfig(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            return services;

        }
    }
}
