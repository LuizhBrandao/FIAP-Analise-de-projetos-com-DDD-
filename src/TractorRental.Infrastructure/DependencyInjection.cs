using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TractorRental.Application.Interfaces;
using TractorRental.Infrastructure.Data;
using TractorRental.Infrastructure.Data.Repositories;

namespace TractorRental.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Configura o EF Core com a Connection String que definiremos no appsettings.json
        services.AddDbContext<TractorRentalDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registra o repositório no container de DI
        services.AddScoped<ITratorRepository, TratorRepository>();

        return services;
    }
}