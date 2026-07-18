using ApiVentas.Configurations;
using ApiVentas.Models;
using ApiVentas.DTOs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ApiVentas.Services;

public class NotaService
{
    private readonly IMongoCollection<Nota> _notas;
    private readonly IMongoCollection<Cobranza> _cobranzas;


    public NotaService(IOptions<MongoSettings> settings)
    {
        var client = new MongoClient(
            settings.Value.ConnectionString);


        var database = client.GetDatabase(
            settings.Value.DatabaseName);


        _notas = database.GetCollection<Nota>("notas");

        _cobranzas = database.GetCollection<Cobranza>("cobranzas");
        
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



    public async Task<NotaDetalleDto?> ObtenerDetalle(string id)
{
    var nota = await _notas
        .Find(x => x.Id == id)
        .FirstOrDefaultAsync();


    if (nota == null)
        return null;


    var cobranzas = await _cobranzas
        .Find(x => x.NotaId == id)
        .ToListAsync();


    var totalCobrado = cobranzas.Sum(x => x.MontoCobrado);


    return new NotaDetalleDto
    {
        Folio = nota.Folio,

        ClienteId = nota.ClienteId,

        TotalVenta = nota.TotalVenta,

        TotalCobrado = totalCobrado,

        Pendiente = nota.TotalVenta - totalCobrado,

        Estado = nota.Estado,

        Cobros = cobranzas.Select(x => new CobroDetalleDto
        {
            AgenteCobroId = x.AgenteCobroId,

            MontoCobrado = x.MontoCobrado,

            FechaCobro = x.FechaCobro,

            Observaciones = x.Observaciones

        }).ToList()
    };
}

}