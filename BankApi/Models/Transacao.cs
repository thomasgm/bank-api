public enum TipoTransacao
{
    Deposito,
    Saque
}
public class Transacao
{
    public int Id { get; set; }

    public int ContaId { get; set; }
    public Conta? Conta { get; set; }

    public decimal Valor { get; set; }

    public TipoTransacao Tipo { get; set; }    // "DEPOSITO" ou "SAQUE"

    public DateTime Data { get; set; } = DateTime.UtcNow;
}