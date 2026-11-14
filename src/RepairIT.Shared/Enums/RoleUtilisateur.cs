using System.Text.Json.Serialization;

namespace RepairIT.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<RoleUtilisateur>))]
public enum RoleUtilisateur
{
    Gestionnaire,
    Administrateur
}
