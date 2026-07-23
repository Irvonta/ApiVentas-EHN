namespace ApiVentas.DTOs;

public class AgentePanelDto
{
    public string AgenteId { get; set; } = string.Empty;

    public decimal Ventas { get; set; }

    public decimal Cobrado { get; set; }

    public decimal Pendiente { get; set; }

    public int Clientes { get; set; }

    public int NotasPendientes { get; set; }

    public decimal AvanceMeta { get; set; }
}