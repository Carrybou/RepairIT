using Microsoft.EntityFrameworkCore;
using RepairIT.Shared.Models;
using RepairIT.UserService.Data;

namespace RepairIT.UserService.Repositories;

public class UtilisateurRepository(UserDbContext context) : IUtilisateurRepository
{
    public Task<List<Utilisateur>> ListerAsync() =>
        context.Utilisateurs.AsNoTracking().OrderBy(u => u.Nom).ThenBy(u => u.Prenom).ToListAsync();

    public Task<Utilisateur?> TrouverParIdAsync(int id) =>
        context.Utilisateurs.FirstOrDefaultAsync(u => u.Id == id);

    public Task<bool> EmailExisteAsync(string email) =>
        context.Utilisateurs.AnyAsync(u => u.Email == email);

    public async Task AjouterAsync(Utilisateur utilisateur)
    {
        context.Utilisateurs.Add(utilisateur);
        await context.SaveChangesAsync();
    }
}
