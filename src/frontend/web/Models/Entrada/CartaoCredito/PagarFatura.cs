using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada referente ao pagamento de uma fatura
    /// </summary>
    public class PagarFatura : BaseModel
    {
        /// <summary>
        /// ID do cartão de crédito
        /// </summary>
        public int IdCartaoCredito { get; set; }

        /// <summary>
        /// Mês da fatura
        /// </summary>
        public int MesFatura { get; set; }

        /// <summary>
        /// Ano da fatura
        /// </summary>
        public int AnoFatura { get; set; }

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
        /// ID da conta onde o lançamento referente ao pagamento da fatura será criado
        /// </summary>
        public int IdContaPagamento { get; set; }

        /// <summary>
        /// ID da pessoa relacionada ao lançamento referente ao pagamento da fatura será criado
        /// </summary>
        public int? IdPessoaPagamento { get; set; }

        /// <summary>
        /// Data do pagamento da fatura
        /// </summary>
        public DateTime DataPagamento { get; set; }

        /// <summary>
        /// Valor do pagamento da fatura
        /// </summary>
        public decimal ValorPagamento { get; set; }
    }
}
