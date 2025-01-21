using ECApp.Application.Interfaces;
using ECApp.Infrastructure.DBContext;
using ECApp.Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration )
        {
            services.AddDbContext<ECDBContext>(options => options.UseSqlServer(configuration.GetConnectionString("ECDBConn")),  ServiceLifetime.Scoped);
            services.AddScoped<IECDbContext>(provider => provider.GetService<ECDBContext>());
            
            return services;
        }
    }
}