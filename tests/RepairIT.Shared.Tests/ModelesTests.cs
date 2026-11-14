using RepairIT.Shared.Enums;
using RepairIT.Shared.Models;

namespace RepairIT.Shared.Tests;

public class ModelesTests
{
    [Fact]
    public void Ordinateur_MettreAJourDepuis_CopieLesChampsModifiablesSansToucherIdNiDate()
    {
        var dateAjout = new DateTime(2026, 1, 1);
        var ordinateur = new Ordinateur { Id = 7, Nom = "Ancien", DateAjout = dateAjout };
        var source = new Ordinateur { Id = 99, Nom = "Nouveau", Marque = "HP", Prix = 500, Stock = 3, Type = TypeOrdinateur.Fixe };

        ordinateur.MettreAJourDepuis(source);

        Assert.Equal(7, ordinateur.Id);
        Assert.Equal(dateAjout, ordinateur.DateAjout);
        Assert.Equal("Nouveau", ordinateur.Nom);
        Assert.Equal("HP", ordinateur.Marque);
        Assert.Equal(500, ordinateur.Prix);
        Assert.Equal(3, ordinateur.Stock);
        Assert.Equal(TypeOrdinateur.Fixe, ordinateur.Type);
    }
}
