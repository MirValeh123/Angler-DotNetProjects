using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Extensions
{
    public static class EFCoreExtensions
    {
        public static IServiceCollection InjectDbContext(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DevDB"));
            });
            return services;
        }
    }
}
