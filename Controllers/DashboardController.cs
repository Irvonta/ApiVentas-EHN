using ApiVentas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiVentas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController : ControllerBase
{
    private readonly DashboardService _service;

    public DashboardController(
        DashboardService service)
    {
        _service = service;
    }

    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumen()
    {
        var resultado =
            await _service.ObtenerResumen();

        return Ok(resultado);
    }

    [HttpGet("agentes")]
public async Task<IActionResult>
    ObtenerAgentes()
{
    var resultado =
        await _service.ObtenerAgentes();

    return Ok(resultado);
}


}