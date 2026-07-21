using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

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
}