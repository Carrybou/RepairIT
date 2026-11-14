using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using RepairIT.CatalogService.Data;

namespace RepairIT.CatalogService.Tests;

public class CatalogDbInitializerTests : BaseDeTestSqlite
{
    [Fact]
    public async Task InitialiserAsync_BaseVide_InsereLeCatalogueDeDemonstrationUneSeuleFois()
    {
        var initializer = new CatalogDbInitializer(Contexte, NullLogger<CatalogDbInitializer>.Instance);

        await initializer.InitialiserAsync();
        var nombreApresPremiereInitialisation = await Contexte.Ordinateurs.CountAsync();
        await initializer.InitialiserAsync();

        Assert.True(nombreApresPremiereInitialisation > 0);
        Assert.Equal(nombreApresPremiereInitialisation, await Contexte.Ordinateurs.CountAsync());
    }
}
