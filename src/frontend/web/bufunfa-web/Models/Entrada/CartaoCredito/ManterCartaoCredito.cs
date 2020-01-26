namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de um cartão de crédito
    /// </summary>
    public class ManterCartaoCredito : BaseModel
    {
        /// <summary>
        /// ID do cartão
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do cartão
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal? ValorLimite { get; set; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; set; }
    }
}
