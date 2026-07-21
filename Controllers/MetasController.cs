using ApiVentas.Models;
using ApiVentas.Services;
using Microsoft.AspNetCore.Mvc;


namespace ApiVentas.Controllers;


[ApiController]
[Route("api/[controller]")]
public class MetasController : ControllerBase
{
    private readonly MetaService _metaService;


    public MetasController(MetaService metaService)
    {
        _metaService = metaService;
    }


    [HttpGet]
    public async Task<ActionResult<List<Meta>>> ObtenerTodas()
    {
        return await _metaService.ObtenerTodas();
    }


    [HttpPost]
    public async Task<IActionResult> Crear(Meta meta)
    {
        await _metaService.Crear(meta);

        return Ok(meta);
    }








    [HttpGet("{agenteId}/resumen")]
public async Task<IActionResult> ObtenerResumen(
    string agenteId)
{
    var resultado =
        await _metaService.ObtenerResumen(agenteId);

    if (resultado == null)
        return NotFound();

    return Ok(resultado);
}
}