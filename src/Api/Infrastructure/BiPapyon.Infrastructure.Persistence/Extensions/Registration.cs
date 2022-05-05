using BiPapyon.Api.Application.Interfaces.Repositories;
using BiPapyon.Infrastructure.Persistence.Context;
using BiPapyon.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BiPapyon.Infrastructure.Persistence.Extensions
{
    public static class Registration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<BiPapyonContext>(options =>
            {
                var connStr = configuration["BiPapyonDbConnectionString"].ToString();

                options.UseSqlServer(connStr, opt =>
                {
                    opt.EnableRetryOnFailure();
                });
            });

            var seedData = new SeedData();

            seedData.SeedAsync(configuration).GetAwaiter().GetResult();

            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
