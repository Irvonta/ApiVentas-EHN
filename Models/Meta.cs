using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiVentas.Models;

public class Meta
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }


    public string AgenteId { get; set; } = string.Empty;


    public int Mes { get; set; }


    public int Anio { get; set; }


    public decimal MetaVentas { get; set; }


    public decimal MetaCobranza { get; set; }


    public bool Activa { get; set; } = true;
}