using ApiVentas.Configurations;
using ApiVentas.DTOs;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class DashboardService
{
    private readonly IMongoCollection<Nota> _notas;
    private readonly IMongoCollection<Cobranza> _cobranzas;
    private readonly IMongoCollection<Cliente> _clientes;
    private readonly IMongoCollection<Agente> _agentes;
    private readonly IMongoCollection<Meta> _metas;

    public DashboardService(
        IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);

        var database = client.GetDatabase(
            settings.Value.DatabaseName);

        _notas = database.GetCollection<Nota>("notas");
        _cobranzas = database.GetCollection<Cobranza>("cobranzas");
        _clientes = database.GetCollection<Cliente>("clientes");
        _agentes = database.GetCollection<Agente>("agentes");
        _metas = database.GetCollection<Meta>("metas");
    }




    public async Task<DashboardResumenDto> ObtenerResumen()
    
{
    
    var notas = await _notas
        .Find(_ => true)
        .ToListAsync();

    var cobranzas = await _cobranzas
        .Find(_ => true)
        .ToListAsync();

    var clientes = await _clientes
        .Find(x => x.Activo)
        .ToListAsync();

    var agentes = await _agentes
        .Find(_ => true)
        .ToListAsync();

    var metas = await _metas
        .Find(x => x.Activa)
        .ToListAsync();

    var ventas = notas.Sum(x => x.TotalVenta);

    var cobrado = cobranzas.Sum(x => x.MontoCobrado);

    return new DashboardResumenDto
    {
        VentasTotales = ventas,
        CobradoTotal = cobrado,
        PendienteTotal = ventas - cobrado,
        ClientesActivos = clientes.Count,
        AgentesActivos = agentes.Count,
        MetasActivas = metas.Count
    };
    
}




public async Task<List<DashboardAgenteDto>>
    ObtenerAgentes()
{
    var agentes = await _agentes
        .Find(_ => true)
        .ToListAsync();

    var notas = await _notas
        .Find(_ => true)
        .ToListAsync();

    var cobranzas = await _cobranzas
        .Find(_ => true)
        .ToListAsync();

    var metas = await _metas
        .Find(x => x.Activa)
        .ToListAsync();

    var resultado = new List<DashboardAgenteDto>();

    foreach (var agente in agentes)
    {
        var notasAgente = notas
            .Where(x => x.AgenteVentaId == agente.AgenteId)
            .ToList();

        var idsNotas = notasAgente
            .Select(x => x.Id)
            .ToList();

        var cobrado = cobranzas
            .Where(x => idsNotas.Contains(x.NotaId))
            .Sum(x => x.MontoCobrado);

        var ventas = notasAgente
            .Sum(x => x.TotalVenta);

        var meta = metas
            .FirstOrDefault(x =>
                x.AgenteId == agente.AgenteId);

        decimal avance = 0;

        if (meta != null &&
            meta.ObjetivoVenta > 0)
        {
            avance =
                (ventas / meta.ObjetivoVenta) * 100;
        }

        resultado.Add(
            new DashboardAgenteDto
            {
                AgenteId = agente.AgenteId,
                Nombre = agente.Nombre,
                Ventas = ventas,
                Cobrado = cobrado,
                Pendiente = ventas - cobrado,
                AvanceMeta = Math.Round(avance, 2)
            });
    }

    return resultado;
}




}
