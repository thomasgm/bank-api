public class Conta
{
    public int Id { get; set; }
    public decimal Saldo { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public bool Sacar(decimal valor)
    {
        if (!PodeSacar(valor))
            return false;

        Saldo -= valor;
        return true;
    }
    public bool PodeSacar(decimal valor)
    {
        if (valor <= 0)
            return false;

        return Saldo >= valor;
    }
}