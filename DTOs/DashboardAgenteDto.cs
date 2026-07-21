namespace ApiVentas.DTOs;

public class DashboardAgenteDto
{
    public string AgenteId { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public decimal Ventas { get; set; }

    public decimal Cobrado { get; set; }

    public decimal Pendiente { get; set; }

    public decimal AvanceMeta { get; set; }
}