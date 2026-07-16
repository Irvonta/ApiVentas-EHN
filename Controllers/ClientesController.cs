using ApiVentas.Models;
using ApiVentas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiVentas.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;


    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }


    [HttpGet]
    public async Task<ActionResult<List<Cliente>>> ObtenerTodos()
    {
        return await _clienteService.ObtenerTodos();
    }


    [HttpPost]
    public async Task<IActionResult> Crear(Cliente cliente)
    {
        await _clienteService.Crear(cliente);

        return Ok(cliente);
    }
}