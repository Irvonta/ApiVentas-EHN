namespace ApiVentas.DTOs;

public class DashboardResumenDto
{
    public decimal VentasTotales { get; set; }

    public decimal CobradoTotal { get; set; }

    public decimal PendienteTotal { get; set; }

    public int ClientesActivos { get; set; }

    public int AgentesActivos { get; set; }

    public int MetasActivas { get; set; }
}