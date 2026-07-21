namespace ApiVentas.DTOs;

public class MetaResumenDto
{
    public string AgenteId { get; set; } = string.Empty;

    public string Periodo { get; set; } = string.Empty;

    public decimal Objetivo { get; set; }

    public decimal Vendido { get; set; }

    public decimal Faltante { get; set; }

    public decimal Avance { get; set; }
}