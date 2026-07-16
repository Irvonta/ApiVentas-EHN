using ApiVentas.Models;
using ApiVentas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiVentas.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CobranzasController : ControllerBase
{
    private readonly CobranzaService _cobranzaService;


    public CobranzasController(CobranzaService cobranzaService)
    {
        _cobranzaService = cobranzaService;
    }


    [HttpGet]
    public async Task<ActionResult<List<Cobranza>>> ObtenerTodas()
    {
        return await _cobranzaService.ObtenerTodas();
    }


    [HttpPost]
    public async Task<IActionResult> Crear(Cobranza cobranza)
    {
        await _cobranzaService.Crear(cobranza);

        return Ok(cobranza);
    }
}