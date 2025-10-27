using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PB.CustomerService.Infrastructure.Data;
using PB.CustomerService.Infrastructure.Repositories;

namespace PB.CustomerService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // Database
            services.AddDbContext<CustomerDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(CustomerDbContext).Assembly.FullName)));

            // Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
