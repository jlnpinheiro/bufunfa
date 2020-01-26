namespace JNogueira.Bufunfa.Web
{
    /// <summary>
    /// Tipo da conta
    /// </summary>
    public enum TipoConta
    {
        ContaCorrente = 1,
        Poupanca = 2,
        RendaFixa = 3,
        RendaVariavel = 4
    }

    /// <summary>
    /// Métodos de pagamento
    /// </summary>
    public enum MetodoPagamento
    {
        Cheque = 1,
        Debito = 2,
        Deposito = 3,
        Transferencia = 4,
        Dinheiro = 5
    }

    /// <summary>
    /// Periodicidade
    /// </summary>
    public enum Periodicidade
    {
        Mensal = 1,
        Trimestral = 3,
        Semestral = 6,
        Anual = 12
    }
}
