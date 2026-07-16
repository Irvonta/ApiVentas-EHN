using ApiVentas.Models;
using ApiVentas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiVentas.Controllers;


[ApiController]
[Route("api/[controller]")]
public class NotasController : ControllerBase
{
    private readonly NotaService _notaService;


    public NotasController(NotaService notaService)
    {
        _notaService = notaService;
    }


    [HttpGet]
    public async Task<ActionResult<List<Nota>>> ObtenerTodas()
    {
        return await _notaService.ObtenerTodas();
    }


    [HttpPost]
    public async Task<IActionResult> Crear(Nota nota)
    {
        await _notaService.Crear(nota);

        return Ok(nota);
    }
}