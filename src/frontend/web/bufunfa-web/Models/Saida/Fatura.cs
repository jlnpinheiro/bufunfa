using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de uma fatura
    /// </summary>
    public class Fatura
    {
        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Cartão de crédito da fatura
        /// </summary>
        public CartaoCredito CartaoCredito { get; set; }

        /// <summary>
        /// Parcelas pertencentes a fatura
        /// </summary>
        public IEnumerable<Parcela> Parcelas { get; set; }

        /// <summary>
        /// Mês da fatura
        /// </summary>
        public int Mes { get; set; }

        /// <summary>
        /// Ano da fatura
        /// </summary>
        public int Ano { get; set; }

        /// <summary>
        /// Valor adicional creditado a fatura
        /// </summary>
        public decimal? ValorAdicionalCredito { get; set; }

        /// <summary>
        /// Observação sobre o valor adicional creditado a fatura
        /// </summary>
        public string ObservacaoCredito { get; set; }

        /// <summary>
        /// Valor adicional debitado a fatura
        /// </summary>
        public decimal? ValorAdicionalDebito { get; set; }

        /// <summary>
        /// Observação sobre o valor adicional debitado a fatura
        /// </summary>
        public string ObservacaoDebito { get; set; }

        /// <summary>
        /// Lançamento relacionado ao pagamento da fatura.
        /// </summary>
        public Lancamento Lancamento { get; set; }

        /// <summary>
        /// Valor total da fatura
        /// </summary>
        public decimal? ValorTotalParcelas { get; set; }

        /// <summary>
        /// Valor total da fatura (com o acréscimo do valor adicional de crédito e substraindo o valor adicional de débito)
        /// </summary>
        public decimal ValorFatura { get; set; }
    }
}
