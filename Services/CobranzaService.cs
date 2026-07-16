using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class CobranzaService
{
    private readonly IMongoCollection<Cobranza> _cobranzas;


    public CobranzaService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);


        var database = client.GetDatabase(
            settings.Value.DatabaseName);


        _cobranzas = database.GetCollection<Cobranza>("cobranzas");
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
    }
}