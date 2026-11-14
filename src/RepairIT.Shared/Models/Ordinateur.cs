using System.ComponentModel.DataAnnotations;
using RepairIT.Shared.Enums;

namespace RepairIT.Shared.Models;

public class Ordinateur
{
    public int Id { get; set; }

    [Required, StringLength(100)]
    public string Nom { get; set; } = string.Empty;

    [Required, StringLength(50)]
    public string Marque { get; set; } = string.Empty;

    [StringLength(100)]
    public string Processeur { get; set; } = string.Empty;

    [Range(1, 1024)]
    public int RamGo { get; set; }

    [Range(1, 65536)]
    public int StockageGo { get; set; }

    public TypeOrdinateur Type { get; set; }

    [Range(0, 100_000)]
    public decimal Prix { get; set; }

    [Range(0, 10_000)]
    public int Stock { get; set; }

    public DateTime DateAjout { get; set; }

    /// <summary>Copie les informations modifiables d'un autre ordinateur (l'Id et la date d'ajout sont conservés).</summary>
    public void MettreAJourDepuis(Ordinateur source)
    {
        Nom = source.Nom;
        Marque = source.Marque;
        Processeur = source.Processeur;
        RamGo = source.RamGo;
        StockageGo = source.StockageGo;
        Type = source.Type;
        Prix = source.Prix;
        Stock = source.Stock;
    }
}
