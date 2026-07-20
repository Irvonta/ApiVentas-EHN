using ApiVentas.Configurations;
using ApiVentas.Models;
using ApiVentas.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class ClienteService
{
    private readonly IMongoCollection<Cliente> _clientes;
    private readonly IMongoCollection<Nota> _notas;
    private readonly IMongoCollection<Cobranza> _cobranzas;

    public ClienteService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);

        var database = client.GetDatabase(
            settings.Value.DatabaseName);

        _clientes = database.GetCollection<Cliente>("clientes");
        _notas = database.GetCollection<Nota>("notas");
        _cobranzas = database.GetCollection<Cobranza>("cobranzas");
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


   public async Task<List<ClienteResumenDto>> ObtenerClientesPorAgente(
    string agenteId)
{
    var clientes = await _clientes
        .Find(x => x.AgenteAsignadoId == agenteId)
        .ToListAsync();


    var resultado = new List<ClienteResumenDto>();


    foreach (var cliente in clientes)
    {
        var notas = await _notas
            .Find(x =>
                x.ClienteId == cliente.ClienteId &&
                x.AgenteVentaId == agenteId)
            .ToListAsync();


        var idsNotas = notas
            .Select(x => x.Id)
            .ToList();


        var cobranzas = await _cobranzas
            .Find(x => idsNotas.Contains(x.NotaId))
            .ToListAsync();


        var comprado = notas.Sum(x => x.TotalVenta);

        var cobrado = cobranzas.Sum(x => x.MontoCobrado);


        resultado.Add(new ClienteResumenDto
        {
            ClienteId = cliente.ClienteId,
            Nombre = cliente.Nombre,
            Telefono = cliente.Telefono,
            Direccion = cliente.Direccion,
            TotalComprado = comprado,
            TotalPendiente = comprado - cobrado
        });
    }


    return resultado;
}
}