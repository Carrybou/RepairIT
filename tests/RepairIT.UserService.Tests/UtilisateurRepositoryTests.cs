using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;
using RepairIT.UserService.Data;
using RepairIT.UserService.Repositories;

namespace RepairIT.UserService.Tests;

public class UtilisateurRepositoryTests : IDisposable
{
    private readonly SqliteConnection connexion = new("DataSource=:memory:");
    private readonly UserDbContext contexte;
    private readonly UtilisateurRepository repository;

    public UtilisateurRepositoryTests()
    {
        connexion.Open();
        contexte = new UserDbContext(new DbContextOptionsBuilder<UserDbContext>().UseSqlite(connexion).Options);
        contexte.Database.EnsureCreated();
        repository = new UtilisateurRepository(contexte);
    }

    private static Utilisateur Creer(string nom, string email) =>
        new() { Nom = nom, Prenom = "Test", Email = email, UserKeycloak = "non-associe" };

    [Fact]
    public async Task ListerAsync_RetourneLesUtilisateursTriesParNom()
    {
        await repository.AjouterAsync(Creer("Zola", "z@repairit.fr"));
        await repository.AjouterAsync(Creer("Albert", "a@repairit.fr"));

        var utilisateurs = await repository.ListerAsync();

        Assert.Equal(["Albert", "Zola"], utilisateurs.Select(u => u.Nom));
    }

    [Fact]
    public async Task EmailExisteAsync_DetecteUnEmailDejaEnregistre()
    {
        await repository.AjouterAsync(Creer("Martin", "alice@repairit.fr"));

        Assert.True(await repository.EmailExisteAsync("alice@repairit.fr"));
        Assert.False(await repository.EmailExisteAsync("bob@repairit.fr"));
    }

    [Fact]
    public async Task AjouterAsync_EmailEnDouble_EstRefuseParLaBase()
    {
        await repository.AjouterAsync(Creer("Martin", "alice@repairit.fr"));

        await Assert.ThrowsAsync<DbUpdateException>(() => repository.AjouterAsync(Creer("Autre", "alice@repairit.fr")));
    }

    public void Dispose()
    {
        contexte.Dispose();
        connexion.Dispose();
        GC.SuppressFinalize(this);
    }
}
