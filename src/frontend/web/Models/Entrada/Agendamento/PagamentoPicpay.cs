using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os pagamentos realizados com Picpay
    /// </summary>
    public class PagarPicpay : BaseModel
    {
        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int IdCartaoCredito { get; set; }

        /// <summary>
        /// Data da compra
        /// </summary>
        public DateTime DataCompra { get; set; }

        /// <summary>
        /// Valor da compra
        /// </summary>
        public decimal ValorCompra { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; set; }
    }
}
