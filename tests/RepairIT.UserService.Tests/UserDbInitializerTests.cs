using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.Shared.Enums;
using RepairIT.UserService.Data;
using RepairIT.UserService.Options;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace RepairIT.UserService.Tests;

public class UserDbInitializerTests : IDisposable
{
    private readonly SqliteConnection connexion = new("DataSource=:memory:");
    private readonly UserDbContext contexte;

    public UserDbInitializerTests()
    {
        connexion.Open();
        contexte = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>().UseSqlite(connexion).Options);
    }

    [Fact]
    public async Task InitialiserAsync_BaseVide_Cree1AdministrateurEt4GestionnairesUneSeuleFois()
    {
        var initializer = new UserDbInitializer(
            contexte,
            MsOptions.Create(new UtilisateurOptions { UserKeycloakParDefaut = "non-associe" }),
            NullLogger<UserDbInitializer>.Instance);

        await initializer.InitialiserAsync();
        await initializer.InitialiserAsync();

        var utilisateurs = await contexte.Utilisateurs.ToListAsync();
        Assert.Equal(5, utilisateurs.Count);
        Assert.Single(utilisateurs, u => u.Role == RoleUtilisateur.Administrateur);
        Assert.All(utilisateurs, u => Assert.Equal("non-associe", u.UserKeycloak));
    }

    public void Dispose()
    {
        contexte.Dispose();
        connexion.Dispose();
        GC.SuppressFinalize(this);
    }
}
