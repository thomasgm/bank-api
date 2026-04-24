using Xunit;
using System; 
using System.Threading.Tasks; 


public class ContaTests
{
    [Fact]
    public void Nao_Deve_Permitir_Saque_Com_Saldo_Insuficiente()
    {
        var conta = new Conta { Saldo = 100 };

        var resultado = conta.PodeSacar(200);

        Assert.False(resultado);
    }

    [Fact]
    public async Task Deposito_Deve_Aumentar_Saldo()
    {
        var conta = new Conta { Saldo = 100 };

        conta.Saldo += 50;

        Assert.Equal(150, conta.Saldo);
    }

    [Fact]
    public async Task Deposito_nao_Deve_Aceitar_Valor_Negativo()
    {
        var conta = new Conta { Saldo = 100 };

        conta.Saldo += -50;

        Assert.Equal(100, conta.Saldo);
    }

     [Fact]
    public void Sacar_ComSaldoSuficiente_DeveReduzirSaldo()
    {
        // Arrange (Preparar)
        var conta = new Conta { Saldo = 100m };
        var valorSaque = 40m;
        // Act (Agir)
        conta.Sacar(valorSaque);
        // Assert (Verificar)
        Assert.Equal(60m, conta.Saldo);
    }
    [Fact]
    public void Sacar_SemSaldoSuficiente_DeveLancarExcecao()
    {
        // Arrange
        var conta = new Conta { Saldo = 50m };
        // Act & Assert (Verifica se estoura erro ao tentar sacar 100)
        Assert.Throws<InvalidOperationException>(() => conta.Sacar(100m));
    }

    [Fact]
    public async Task SaquesSimultaneos_NaoDevemDeixarSaldoNegativo()
    {
        // Arrange: Saldo de 100 reais
        var conta = new Conta { Saldo = 100 };
        
        // Act: Dispara duas tarefas ao mesmo tempo tentando tirar 100 reais cada
        var task1 = Task.Run(() => conta.Sacar(100));
        var task2 = Task.Run(() => conta.Sacar(100));

        // Assert: Uma deve ter sucesso e a outra DEVE falhar
        await Assert.ThrowsAnyAsync<Exception>(async () => {
            await Task.WhenAll(task1, task2);
        });
        
        Assert.True(conta.Saldo >= 0); // O saldo nunca pode ser -100
    }
}