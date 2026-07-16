using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class NotaService
{
    private readonly IMongoCollection<Nota> _notas;


    public NotaService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);


        var database = client.GetDatabase(
            settings.Value.DatabaseName);


        _notas = database.GetCollection<Nota>("notas");
    }


    public async Task<List<Nota>> ObtenerTodas()
    {
        return await _notas
            .Find(_ => true)
            .ToListAsync();
    }


    public async Task Crear(Nota nota)
    {
        await _notas.InsertOneAsync(nota);
    }
}