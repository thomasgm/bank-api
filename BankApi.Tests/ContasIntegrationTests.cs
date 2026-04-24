using Xunit;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks; 
using Microsoft.EntityFrameworkCore.Diagnostics;


public class ContasIntegrationTests
{
    [Fact]
    public async Task Deposito_DeveAtualizarBancoDeDados()
    {
        // Arrange: Criar banco em memória para o teste
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TesteDeposito")
            .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning)) // <--- adicionado pra ignorar warning de transação
            .Options;

        using var context = new AppDbContext(options);
        var controller = new ContasController(context);
        
        var conta = new Conta { Id = 1, Saldo = 10, UsuarioId = 1 };
        context.Contas.Add(conta);
        await context.SaveChangesAsync();

        var request = new DepositoRequest { ContaId = 1, Valor = 50 };

        // Act
        await controller.Deposito(request);

        // Assert
        var contaNoBanco = await context.Contas.FindAsync(1);
        Assert.NotNull(contaNoBanco); 
        Assert.Equal(60, contaNoBanco.Saldo);
        Assert.Single(context.Transacoes); // Garante que criou 1 registro de transação
    }
}
