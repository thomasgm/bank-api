using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly AppDbContext _context;

    public UsuariosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usuarios = await _context.Usuarios.ToListAsync();
        return Ok(usuarios);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Usuario usuario)
    {
        if (usuario == null)
            return BadRequest("Usuário inválido");

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = usuario.Id }, usuario);
    }
}