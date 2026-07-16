using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiVentas.Models;

public class Cliente
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string ClienteId { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string AgenteAsignadoId { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}