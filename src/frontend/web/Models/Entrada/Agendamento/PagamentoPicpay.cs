using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os pagamentos realizados com a funcionalidade "Pagar com..."
    /// </summary>
    public class Pagar : BaseModel
    {
        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int IdCartaoCredito { get; set; }

        /// <summary>
        /// Nome da pessoa para quem será feito o pagamento
        /// </summary>
        public string NomePessoa { get; set; }

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
