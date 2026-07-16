using ApiVentas.Configurations;
using ApiVentas.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class ClienteService
{
    private readonly IMongoCollection<Cliente> _clientes;

    public ClienteService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);

        var database = client.GetDatabase(
            settings.Value.DatabaseName);

        _clientes = database.GetCollection<Cliente>("clientes");
    }


    public async Task<List<Cliente>> ObtenerTodos()
    {
        return await _clientes
            .Find(_ => true)
            .ToListAsync();
    }


    public async Task Crear(Cliente cliente)
    {
        await _clientes.InsertOneAsync(cliente);
    }
}