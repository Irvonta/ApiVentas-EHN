namespace ApiVentas.DTOs;

public class ClienteResumenDto
{
    public string ClienteId { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public decimal TotalComprado { get; set; }

    public decimal TotalPendiente { get; set; }
}