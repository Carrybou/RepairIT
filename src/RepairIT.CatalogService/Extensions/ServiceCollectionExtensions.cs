using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.CatalogService.Repositories;
using RepairIT.CatalogService.Services;

namespace RepairIT.CatalogService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AjouterCatalogue(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("CatalogDb")));

        // TimeProvider injecté plutôt que DateTime.UtcNow : les dates deviennent contrôlables dans les tests.
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IOrdinateurRepository, OrdinateurRepository>();
        services.AddScoped<IOrdinateurService, OrdinateurService>();
        services.AddScoped<CatalogDbInitializer>();

        return services;
    }

    public static async Task InitialiserBaseDeDonneesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<CatalogDbInitializer>().InitialiserAsync();
    }
}
