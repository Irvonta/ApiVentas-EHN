using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiVentas.Models;

public class Meta
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }


    public string AgenteId { get; set; } = string.Empty;


    // Ejemplo: 2026-07
    public string Periodo { get; set; } = string.Empty;


    public decimal ObjetivoVenta { get; set; }


    public bool Activa { get; set; } = true;
}