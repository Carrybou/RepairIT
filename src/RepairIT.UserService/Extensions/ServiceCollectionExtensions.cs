using Microsoft.EntityFrameworkCore;
using RepairIT.UserService.Data;
using RepairIT.UserService.Options;
using RepairIT.UserService.Repositories;
using RepairIT.UserService.Services;

namespace RepairIT.UserService.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AjouterGestionUtilisateurs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<UserDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("UserDb")));

        services.Configure<UtilisateurOptions>(configuration.GetSection(UtilisateurOptions.Section));
        // TimeProvider injecté plutôt que DateTime.UtcNow : les dates deviennent contrôlables dans les tests.
        services.AddSingleton(TimeProvider.System);
        services.AddScoped<IUtilisateurRepository, UtilisateurRepository>();
        services.AddScoped<IUtilisateurService, UtilisateurService>();
        services.AddScoped<UserDbInitializer>();

        return services;
    }

    public static async Task InitialiserBaseDeDonneesAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        await scope.ServiceProvider.GetRequiredService<UserDbInitializer>().InitialiserAsync();
    }
}
