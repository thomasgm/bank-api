using Xunit; 
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class BankApiFunctionalTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BankApiFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetExtrato_ContaInexistente_DeveRetornar404()
    {
        // Act: Tenta acessar uma conta que não existe
        var response = await _client.GetAsync("/api/contas/999/extrato");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    
    [Theory]
    [InlineData(-100)] // Valor negativo
    [InlineData(0)]    // Valor zero
    public async Task Deposito_ValoresInvalidos_DeveRetornarBadRequest(decimal valorInvalido)
    {
        // Arrange
        var request = new DepositoRequest { ContaId = 1, Valor = valorInvalido };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contas/deposito", request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
