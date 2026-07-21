namespace ApiVentas.DTOs;

public class ClienteAnalisisDto
{
    public string ClienteId { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public decimal ComprasTotales { get; set; }

    public int NotasTotales { get; set; }

    public int NotasPagadas { get; set; }

    public double PromedioDiasPago { get; set; }

    public string Clasificacion { get; set; } = string.Empty;
}