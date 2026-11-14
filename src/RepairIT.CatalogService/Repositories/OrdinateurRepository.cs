using Microsoft.EntityFrameworkCore;
using RepairIT.CatalogService.Data;
using RepairIT.Shared.Models;

namespace RepairIT.CatalogService.Repositories;

public class OrdinateurRepository(CatalogDbContext context) : IOrdinateurRepository
{
    // AsNoTracking : simple lecture, EF Core n'a pas besoin de suivre les entités retournées.
    public Task<List<Ordinateur>> ListerAsync() =>
        context.Ordinateurs.AsNoTracking().OrderBy(o => o.Marque).ThenBy(o => o.Nom).ToListAsync();

    public Task<Ordinateur?> TrouverParIdAsync(int id) =>
        context.Ordinateurs.FirstOrDefaultAsync(o => o.Id == id);

    public async Task AjouterAsync(Ordinateur ordinateur)
    {
        context.Ordinateurs.Add(ordinateur);
        await context.SaveChangesAsync();
    }

    public async Task SupprimerAsync(Ordinateur ordinateur)
    {
        context.Ordinateurs.Remove(ordinateur);
        await context.SaveChangesAsync();
    }

    public Task SauvegarderAsync() => context.SaveChangesAsync();
}
