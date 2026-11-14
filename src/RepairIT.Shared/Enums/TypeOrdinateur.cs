using System.Text.Json.Serialization;

namespace RepairIT.Shared.Enums;

[JsonConverter(typeof(JsonStringEnumConverter<TypeOrdinateur>))]
public enum TypeOrdinateur
{
    Portable,
    Fixe,
    ToutEnUn,
    Serveur
}
