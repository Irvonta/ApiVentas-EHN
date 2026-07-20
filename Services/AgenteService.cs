using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ApiVentas.DTOs;

namespace ApiVentas.Services;

public class AgenteService
{

    private readonly IMongoCollection<Nota> _notas;
    private readonly IMongoCollection<Cobranza> _cobranzas;
    private readonly IMongoCollection<Agente> _agentes;

    public AgenteService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);

        var database = client.GetDatabase(
            settings.Value.DatabaseName);

        _agentes = database.GetCollection<Agente>("agentes");
        _notas = database.GetCollection<Nota>("notas");
        _cobranzas = database.GetCollection<Cobranza>("cobranzas");
    }

    public async Task<List<Agente>> ObtenerTodos()
    {
        return await _agentes.Find(_ => true).ToListAsync();
    }

    public async Task Crear(Agente agente)
    {
        await _agentes.InsertOneAsync(agente);
    }
    public async Task<AgenteResumenDto> ObtenerResumen(string agenteId)
{
    var notas = await _notas
        .Find(x => x.AgenteVentaId == agenteId)
        .ToListAsync();

    var idsNotas = notas
        .Select(x => x.Id)
        .ToList();

    var cobranzas = await _cobranzas
        .Find(x => idsNotas.Contains(x.NotaId))
        .ToListAsync();

    var ventas = notas.Sum(x => x.TotalVenta);

    var cobrado = cobranzas.Sum(x => x.MontoCobrado);

    return new AgenteResumenDto
    {
        AgenteId = agenteId,
        CantidadNotas = notas.Count,
        Ventas = ventas,
        Cobrado = cobrado,
        Pendiente = ventas - cobrado
    };
    
}



public async Task<AgenteCobranzaDto> ObtenerCobranza(
    string agenteId)
{
    var cobranzas = await _cobranzas
        .Find(x => x.AgenteCobroId == agenteId)
        .ToListAsync();

    return new AgenteCobranzaDto
    {
        AgenteId = agenteId,
        Movimientos = cobranzas.Count,
        TotalCobrado = cobranzas.Sum(x => x.MontoCobrado)
    };
}
}