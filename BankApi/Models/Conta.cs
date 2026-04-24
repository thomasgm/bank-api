public class Conta
{
    public int Id { get; set; }
    public decimal Saldo { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public bool Sacar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do saque deve ser positivo.");
        if (Saldo < valor)
            throw new InvalidOperationException("Saldo insuficiente para realizar o saque.");

        Saldo -= valor;
        return true;
    }
    public bool PodeSacar(decimal valor)
    {
        if (valor <= 0)
            return false;

        return Saldo >= valor;
    }
    public void Depositar(decimal valor)
    {
        if (valor <= 0)
            throw new ArgumentException("O valor do depósito deve ser positivo.");
        Saldo += valor;
    }
}