namespace ApiVentas.DTOs;

public class CobranzaPendienteDto
{
    public string Folio { get; set; } = string.Empty;

    public string ClienteId { get; set; } = string.Empty;

    public decimal TotalVenta { get; set; }

    public decimal TotalCobrado { get; set; }

    public decimal Pendiente { get; set; }

    public int DiasPendiente { get; set; }
}