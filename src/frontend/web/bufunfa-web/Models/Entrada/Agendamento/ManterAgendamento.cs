using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de um agendamento
    /// </summary>
    public class ManterAgendamento : BaseModel
    {
        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string NomePessoa { get; set; }

        /// <summary>
        /// Tipo do método de pagamento das parcelas
        /// </summary>
        public MetodoPagamento TipoMetodoPagamento { get; set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        public decimal ValorParcela { get; set; }

        /// <summary>
        /// Data da primeira parcela do agendamento
        /// </summary>
        public DateTime DataPrimeiraParcela { get; set; }

        /// <summary>
        /// Quantidade de parcelas
        /// </summary>
        public int QuantidadeParcelas { get; set; }

        /// <summary>
        /// Periodicidade das parcelas
        /// </summary>
        public Periodicidade PeriodicidadeParcelas { get; set; }
    }
}
