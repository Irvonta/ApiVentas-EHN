using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class MetaService
{
    private readonly IMongoCollection<Meta> _metas;


    public MetaService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);


        var database = client.GetDatabase(
            settings.Value.DatabaseName);


        _metas = database.GetCollection<Meta>("metas");
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
}