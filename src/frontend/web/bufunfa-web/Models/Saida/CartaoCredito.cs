namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de um cartão de crédito
    /// </summary>
    public class CartaoCredito
    {
        /// <summary>
        /// ID do cartão de crédito
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do cartão de crédito
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal ValorLimite { get; set; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; set; }

        /// <summary>
        /// Valor do limite disponível
        /// </summary>
        public decimal? ValorLimiteDisponivel { get; set; }

        public string ObterValorLimiteEmReais() => this.ValorLimite.ToString("c2");

        public string ObterValorLimiteDisponivelEmReais() => this.ValorLimiteDisponivel?.ToString("c2");
    }
}
