public class Conta
{
    public int Id { get; set; }
    public decimal Saldo { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}