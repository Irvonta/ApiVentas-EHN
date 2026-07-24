using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using ApiVentas.DTOs;

namespace ApiVentas.Services;

public class CobranzaService
{
    private readonly IMongoCollection<Cobranza> _cobranzas;
    private readonly IMongoCollection<Nota> _notas;

    public CobranzaService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);


        var database = client.GetDatabase(
            settings.Value.DatabaseName);


        _cobranzas = database.GetCollection<Cobranza>("cobranzas");
        _notas = database.GetCollection<Nota>("notas");
    }


    public async Task<List<Cobranza>> ObtenerTodas()
    {
        return await _cobranzas
            .Find(_ => true)
            .ToListAsync();
    }


    public async Task Crear(Cobranza cobranza)
{
    await _cobranzas.InsertOneAsync(cobranza);

    var nota = await _notas
        .Find(x => x.Id == cobranza.NotaId)
        .FirstOrDefaultAsync();

    if (nota == null)
        return;

    var totalCobrado = await _cobranzas
        .Find(x => x.NotaId == cobranza.NotaId)
        .ToListAsync();

    var sumaCobrada = totalCobrado
        .Sum(x => x.MontoCobrado);

    if (sumaCobrada >= nota.TotalVenta)
    {
        var update = Builders<Nota>
            .Update
            .Set(x => x.Estado, "Pagada")
            .Set(x => x.FechaLiquidacion,
                 DateTime.Now);

        await _notas.UpdateOneAsync(
            x => x.Id == nota.Id,
            update);
    }
}


public async Task<List<CobranzaPendienteDto>>
    ObtenerPendientes()
{
    var notas = await _notas
        .Find(x => x.Estado == "Pendiente")
        .ToListAsync();

    var resultado =
        new List<CobranzaPendienteDto>();

    foreach (var nota in notas)
    {
        var cobranzas = await _cobranzas
            .Find(x => x.NotaId == nota.Id)
            .ToListAsync();

        var totalCobrado =
            cobranzas.Sum(x => x.MontoCobrado);

        resultado.Add(
            new CobranzaPendienteDto
            {
                Folio = nota.Folio,
                ClienteId = nota.ClienteId,
                TotalVenta = nota.TotalVenta,
                TotalCobrado = totalCobrado,
                Pendiente =
                    nota.TotalVenta - totalCobrado,
                DiasPendiente =
                    (DateTime.Now -
                     nota.FechaVenta).Days
            });
    }

    return resultado
        .OrderByDescending(x => x.DiasPendiente)
        .ToList();
}


}