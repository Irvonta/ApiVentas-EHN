using ApiVentas.Models;
using ApiVentas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiVentas.Controllers;
[ApiController]
[Route("api/[controller]")]
public class AgentesController : ControllerBase
{
    private readonly AgenteService _service;

    public AgentesController(
        AgenteService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Obtener()
    {
        return Ok(await _service.ObtenerTodos());
    }

    [HttpPost]
    public async Task<IActionResult> Crear(
        Agente agente)
    {
        await _service.Crear(agente);

        return Ok(agente);
    }

    [HttpGet("{agenteId}/resumen")]
    public async Task<IActionResult> ObtenerResumen(
        string agenteId)
    {
        var resultado =
            await _service.ObtenerResumen(agenteId);

        return Ok(resultado);
    }


[HttpGet("{agenteId}/cobranzas")]
public async Task<IActionResult> ObtenerCobranza(
    string agenteId)
{
    var resultado =
        await _service.ObtenerCobranza(agenteId);

    return Ok(resultado);
}

}