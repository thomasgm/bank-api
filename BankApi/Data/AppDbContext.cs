using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Conta> Contas { get; set; }
    public DbSet<Transacao> Transacoes { get; set; }
}