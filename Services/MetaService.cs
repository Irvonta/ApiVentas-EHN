using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ApiVentas.DTOs;



namespace ApiVentas.Services;

public class MetaService
{
    private readonly IMongoCollection<Meta> _metas;
    private readonly IMongoCollection<Nota> _notas;

    public MetaService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);


        var database = client.GetDatabase(
            settings.Value.DatabaseName);


        _metas = database.GetCollection<Meta>("metas");
        _notas = database.GetCollection<Nota>("notas");

    }


    public async Task<List<Meta>> ObtenerTodas()
    {
        return await _metas
            .Find(_ => true)
            .ToListAsync();
    }


    public async Task Crear(Meta meta)
    {
        await _metas.InsertOneAsync(meta);
    }


    public async Task<MetaResumenDto?> ObtenerResumen(
    string agenteId)
{
    var meta = await _metas
        .Find(x =>
            x.AgenteId == agenteId &&
            x.Activa)
        .FirstOrDefaultAsync();

    if (meta == null)
        return null;


    var notas = await _notas
        .Find(x => x.AgenteVentaId == agenteId)
        .ToListAsync();


    var vendido = notas.Sum(x => x.TotalVenta);

    var faltante = meta.ObjetivoVenta - vendido;

    var avance =
        meta.ObjetivoVenta == 0
        ? 0
        : (vendido / meta.ObjetivoVenta) * 100;


    return new MetaResumenDto
    {
        AgenteId = agenteId,
        Periodo = meta.Periodo,
        Objetivo = meta.ObjetivoVenta,
        Vendido = vendido,
        Faltante = faltante,
        Avance = Math.Round(avance, 2)
    };
}
}