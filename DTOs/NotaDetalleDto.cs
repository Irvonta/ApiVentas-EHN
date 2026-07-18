namespace ApiVentas.DTOs;

public class NotaDetalleDto
{
    public string Folio { get; set; } = string.Empty;

    public string ClienteId { get; set; } = string.Empty;

    public decimal TotalVenta { get; set; }

    public decimal TotalCobrado { get; set; }

    public decimal Pendiente { get; set; }

    public string Estado { get; set; } = string.Empty;

    public List<CobroDetalleDto> Cobros { get; set; } = new();
}


public class CobroDetalleDto
{
    public string AgenteCobroId { get; set; } = string.Empty;

    public decimal MontoCobrado { get; set; }

    public DateTime FechaCobro { get; set; }

    public string Observaciones { get; set; } = string.Empty;
}
