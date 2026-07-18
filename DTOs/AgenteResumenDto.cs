namespace ApiVentas.DTOs;

public class AgenteResumenDto
{
    public string AgenteId { get; set; } = string.Empty;

    public int CantidadNotas { get; set; }

    public decimal Ventas { get; set; }

    public decimal Cobrado { get; set; }

    public decimal Pendiente { get; set; }
}