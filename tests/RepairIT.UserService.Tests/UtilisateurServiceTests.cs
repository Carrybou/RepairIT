using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using RepairIT.Shared.Enums;
using RepairIT.Shared.Requests;
using RepairIT.UserService.Options;
using RepairIT.UserService.Repositories;
using RepairIT.UserService.Services;
using MsOptions = Microsoft.Extensions.Options.Options;

namespace RepairIT.UserService.Tests;

public class UtilisateurServiceTests
{
    private const string UserKeycloakParDefaut = "en-attente-keycloak";
    private static readonly DateTimeOffset Maintenant = new(2026, 11, 14, 10, 0, 0, TimeSpan.Zero);

    private readonly Mock<IUtilisateurRepository> repository = new();
    private readonly UtilisateurService service;

    public UtilisateurServiceTests()
    {
        var horloge = new Mock<TimeProvider>();
        horloge.Setup(h => h.GetUtcNow()).Returns(Maintenant);

        service = new UtilisateurService(
            repository.Object,
            MsOptions.Create(new UtilisateurOptions { UserKeycloakParDefaut = UserKeycloakParDefaut }),
            horloge.Object,
            NullLogger<UtilisateurService>.Instance);
    }

    [Fact]
    public async Task CreerAsync_UtiliseLeUserKeycloakParDefautEtNormaliseLesChamps()
    {
        var requete = new CreerUtilisateurRequest(" Dupont ", " Jean ", " Jean.Dupont@RepairIT.fr ", RoleUtilisateur.Gestionnaire);

        var utilisateur = await service.CreerAsync(requete);

        Assert.Equal(UserKeycloakParDefaut, utilisateur.UserKeycloak);
        Assert.Equal("Dupont", utilisateur.Nom);
        Assert.Equal("Jean", utilisateur.Prenom);
        Assert.Equal("jean.dupont@repairit.fr", utilisateur.Email);
        Assert.Equal(Maintenant.UtcDateTime, utilisateur.DateCreation);
        repository.Verify(r => r.AjouterAsync(utilisateur), Times.Once);
    }

    [Fact]
    public async Task EmailDejaUtiliseAsync_InterrogeLeRepositoryAvecLEmailNormalise()
    {
        repository.Setup(r => r.EmailExisteAsync("alice@repairit.fr")).ReturnsAsync(true);

        Assert.True(await service.EmailDejaUtiliseAsync("  ALICE@repairit.fr"));
    }
}
