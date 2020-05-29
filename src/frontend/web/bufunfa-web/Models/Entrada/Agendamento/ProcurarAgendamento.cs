using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para a procura de agendamentos
    /// </summary>
    public class ProcurarAgendamento : BaseModel
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// ID da categoria
        /// </summary>
        public int? IdCategoria { get; set; }

        /// <summary>
        /// ID do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; set; }

        /// <summary>
        /// ID da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Data ínicio da parcela do agendamento
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataInicioParcela { get; set; }

        /// <summary>
        /// Data fim da parcela do agendamento
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataFimParcela { get; set; }

        /// <summary>
        /// Indica se os agendamentos concluídos também deverão ser retornados
        /// </summary>
        public bool? Concluido { get; set; }
    }
}
