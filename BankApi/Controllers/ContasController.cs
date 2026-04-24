using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ContasController : ControllerBase
{
    private readonly AppDbContext _context;

    public ContasController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
public async Task<IActionResult> CriarConta([FromBody] CriarContaRequest request)
{
    var usuario = await _context.Usuarios.FindAsync(request.UsuarioId);

    if (usuario == null)
        return NotFound("Usuário não encontrado");

    var conta = new Conta
    {
        UsuarioId = request.UsuarioId,
        Saldo = 0
    };

    _context.Contas.Add(conta);
    await _context.SaveChangesAsync();

    return Ok(conta);
}
[HttpPost("deposito")]
public async Task<IActionResult> Deposito([FromBody] DepositoRequest request)
{
    if (request == null || request.Valor <= 0)
        return BadRequest("Valor inválido");

    var conta = await _context.Contas.FindAsync(request.ContaId);

    if (conta == null)
        return NotFound($"Conta {request.ContaId} não encontrada");

    await using var transaction = await _context.Database.BeginTransactionAsync();
    try
    {
         conta.Saldo += request.Valor;
        var transacao = new Transacao
        {
            ContaId = conta.Id,
            Valor = request.Valor,
            Tipo = TipoTransacao.Deposito,
            Data = DateTime.UtcNow
        };
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return Ok(conta);
    }
    catch
    {
        await transaction.RollbackAsync();
        return StatusCode(500, "Erro ao processar depósito");
    }
}
[HttpPost("saque")]
public async Task<IActionResult> Saque([FromBody] SaqueRequest request)
{
    if (request == null || request.Valor <= 0)
        return BadRequest("Valor inválido");

    var conta = await _context.Contas.FindAsync(request.ContaId);

    if (conta == null)
        return NotFound($"Conta {request.ContaId} não encontrada");

    if (conta.Saldo < request.Valor)
        return BadRequest("Saldo insuficiente");


    await using var transaction = await _context.Database.BeginTransactionAsync();
    try {
        //conta.Saldo -= request.Valor;
        conta.Sacar (request.Valor);
        var transacao = new Transacao
        {
            ContaId = conta.Id,
            Valor = request.Valor,
            Tipo = TipoTransacao.Saque,
            Data = DateTime.UtcNow
        };
        _context.Transacoes.Add(transacao);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
        return Ok(conta);
    } catch
    {
        await transaction.RollbackAsync();
        return StatusCode(500, "Erro ao processar saque");
    }
}

[HttpGet("{contaId}/extrato")]
public async Task<IActionResult> Extrato(int contaId)
{
    var conta = await _context.Contas
        .Include(c => c.Usuario)
        .FirstOrDefaultAsync(c => c.Id == contaId);

    if (conta == null)
        return NotFound("Conta não encontrada");

    var transacoes = await _context.Transacoes
        .Where(t => t.ContaId == contaId)
        .OrderByDescending(t => t.Data)
        .ToListAsync();

    
    var resultado = new
    {
        ContaId = conta.Id,
        Usuario = conta.Usuario?.Nome,
        Saldo = conta.Saldo,
        Transacoes = transacoes.Select(t => new
        {
            
            t.Id,
            Tipo = t.Tipo.ToString(),
            Valor = t.Tipo == TipoTransacao.Saque ? -t.Valor : t.Valor,
            Data = t.Data.ToString("yyyy-MM-dd HH:mm:ss")
        })
    };

    return Ok(resultado);
}
}