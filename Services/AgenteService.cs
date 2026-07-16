using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class AgenteService
{
    private readonly IMongoCollection<Agente> _agentes;

    public AgenteService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);

        var database = client.GetDatabase(
            settings.Value.DatabaseName);

        _agentes = database.GetCollection<Agente>("agentes");
    }

    public async Task<List<Agente>> ObtenerTodos()
    {
        return await _agentes.Find(_ => true).ToListAsync();
    }

    public async Task Crear(Agente agente)
    {
        await _agentes.InsertOneAsync(agente);
    }
}