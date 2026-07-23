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
public async Task<ClienteAnalisisDto?> ObtenerAnalisis(
    string clienteId)
{
    var cliente = await _clientes
        .Find(x => x.ClienteId == clienteId)
        .FirstOrDefaultAsync();


    if (cliente == null)
        return null;


    var notas = await _notas
        .Find(x => x.ClienteId == clienteId)
        .ToListAsync();


    var pagadas = notas
        .Where(x => x.FechaLiquidacion != null)
        .ToList();


    double promedioDias = 0;


    if (pagadas.Count > 0)
    {
        promedioDias = pagadas
            .Average(x =>
                (x.FechaLiquidacion.Value -
                 x.FechaVenta).TotalDays);
    }


    string clasificacion;


    if (promedioDias <= 15)
    {
        clasificacion = "Cliente Bueno";
    }
    else if (promedioDias <= 45)
    {
        clasificacion = "Cliente Regular";
    }
    else
    {
        clasificacion = "Cliente de Riesgo";
    }


    return new ClienteAnalisisDto
    {
        ClienteId = cliente.ClienteId,
        Nombre = cliente.Nombre,
        ComprasTotales = notas.Sum(x => x.TotalVenta),
        NotasTotales = notas.Count,
        NotasPagadas = pagadas.Count,
        PromedioDiasPago = Math.Round(promedioDias,2),
        Clasificacion = clasificacion
    };
}
public async Task<List<ClienteAnalisisDto>>
    ObtenerAnalisisTodos()
{
    var clientes = await _clientes
        .Find(_ => true)
        .ToListAsync();

    var resultado =
        new List<ClienteAnalisisDto>();

    foreach (var cliente in clientes)
    {
        var notas = await _notas
            .Find(x => x.ClienteId ==
                       cliente.ClienteId)
            .ToListAsync();

        var pagadas = notas
            .Where(x => x.FechaLiquidacion != null)
            .ToList();

        double promedioDias = 0;

        if (pagadas.Count > 0)
        {
            promedioDias = pagadas
                .Average(x =>
                    (x.FechaLiquidacion!.Value -
                     x.FechaVenta).TotalDays);
        }

        string clasificacion;

        if (promedioDias <= 15)
            clasificacion = "Cliente Bueno";
        else if (promedioDias <= 45)
            clasificacion = "Cliente Regular";
        else
            clasificacion = "Cliente de Riesgo";

        resultado.Add(new ClienteAnalisisDto
        {
            ClienteId = cliente.ClienteId,
            Nombre = cliente.Nombre,
            ComprasTotales =
                notas.Sum(x => x.TotalVenta),
            NotasTotales = notas.Count,
            NotasPagadas = pagadas.Count,
            PromedioDiasPago =
                Math.Round(promedioDias, 2),
            Clasificacion = clasificacion
        });
    }

    return resultado;
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





public async Task<List<ClientePendienteDto>>
    ObtenerPendientes()
{
    var clientes = await _clientes
        .Find(_ => true)
        .ToListAsync();

    var resultado =
        new List<ClientePendienteDto>();

    foreach (var cliente in clientes)
    {
        var notas = await _notas
            .Find(x => x.ClienteId ==
                       cliente.ClienteId)
            .ToListAsync();

        var idsNotas = notas
            .Select(x => x.Id)
            .ToList();

        var cobranzas = await _cobranzas
            .Find(x => idsNotas.Contains(x.NotaId))
            .ToListAsync();

        var comprado =
            notas.Sum(x => x.TotalVenta);

        var cobrado =
            cobranzas.Sum(x => x.MontoCobrado);

        var pendiente =
            comprado - cobrado;

        resultado.Add(
            new ClientePendienteDto
            {
                ClienteId = cliente.ClienteId,
                Nombre = cliente.Nombre,
                TotalComprado = comprado,
                TotalCobrado = cobrado,
                Pendiente = pendiente
            });
    }

    return resultado
        .OrderByDescending(x => x.Pendiente)
        .ToList();
}





public async Task<List<NotaDetalleDto>>
    ObtenerNotasCliente(string clienteId)
{
    var notas = await _notas
        .Find(x => x.ClienteId == clienteId)
        .ToListAsync();

    var resultado =
        new List<NotaDetalleDto>();

    foreach (var nota in notas)
    {
        var cobranzas = await _cobranzas
            .Find(x => x.NotaId == nota.Id)
            .ToListAsync();

        resultado.Add(new NotaDetalleDto
        {
            Folio = nota.Folio,
            ClienteId = nota.ClienteId,
            TotalVenta = nota.TotalVenta,
            TotalCobrado =
                cobranzas.Sum(x => x.MontoCobrado),
            Pendiente =
                nota.TotalVenta -
                cobranzas.Sum(x => x.MontoCobrado),
            Estado = nota.Estado,

            Cobros = cobranzas.Select(x =>
                new CobroDetalleDto
                {
                    AgenteCobroId =
                        x.AgenteCobroId,
                    MontoCobrado =
                        x.MontoCobrado,
                    FechaCobro =
                        x.FechaCobro,
                    Observaciones =
                        x.Observaciones
                }).ToList()
        });
    }

    return resultado;
}




}