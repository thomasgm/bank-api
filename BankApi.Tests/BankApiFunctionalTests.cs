using Xunit; 
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;


public class BankApiFunctionalTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BankApiFunctionalTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
    {
        builder.ConfigureServices(services =>
        {
            // 1. Remove TUDO que for relacionado ao AppDbContext (o banco e as opções)
            var descriptors = services.Where(
                d => d.ServiceType.Name.Contains("DbContext") || 
                     d.ServiceType == typeof(AppDbContext) ||
                     d.ServiceType == typeof(DbContextOptions<AppDbContext>)).ToList();
            foreach (var d in descriptors) services.Remove(d);
            // 2. Cria um "provedor de serviços" novo e isolado apenas para o banco em memória
            // Isso evita que ele se misture com os restos do SQL Server
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();
            // 3. Adiciona o banco em memória usando esse provedor isolado
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseInMemoryDatabase("FunctionalTestsDb");
                options.UseInternalServiceProvider(serviceProvider); // banco em memória isolado só para os testes funcionais
            });
        });
    }).CreateClient();
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
