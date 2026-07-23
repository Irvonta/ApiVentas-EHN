namespace ApiVentas.DTOs;

public class ClientePendienteDto
{
    public string ClienteId { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public decimal TotalComprado { get; set; }

    public decimal TotalCobrado { get; set; }

    public decimal Pendiente { get; set; }
}