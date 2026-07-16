using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiVentas.Models;

public class Agente
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string AgenteId { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}