namespace ApiVentas.DTOs;

public class AgenteCobranzaDto
{
    public string AgenteId { get; set; } = string.Empty;

    public int Movimientos { get; set; }

    public decimal TotalCobrado { get; set; }
}