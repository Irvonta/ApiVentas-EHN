using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiVentas.Models;

public class Cobranza
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string Folio { get; set; } = string.Empty;

    public decimal MontoCobrado { get; set; }

    public DateTime FechaCobro { get; set; } = DateTime.Now;

    public string AgenteCobranzaId { get; set; } = string.Empty;

    public string Observaciones { get; set; } = string.Empty;
}