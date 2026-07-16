using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ApiVentas.Models;

public class Nota
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }


    // Ejemplo: J1057
    public string Folio { get; set; } = string.Empty;


    // Año de la nota, para reiniciar consecutivos cada año
    public int Anio { get; set; } = DateTime.Now.Year;


    // Cliente al que pertenece la venta
    public string ClienteId { get; set; } = string.Empty;


    // Total de la nota
    public decimal TotalVenta { get; set; }


    // Fecha en que se realizó la venta
    public DateTime FechaVenta { get; set; } = DateTime.Now;


    // Estado de la nota
    // Pendiente = aún no se cobra completa
    // Pagada = liquidada
    public string Estado { get; set; } = "Pendiente";
}