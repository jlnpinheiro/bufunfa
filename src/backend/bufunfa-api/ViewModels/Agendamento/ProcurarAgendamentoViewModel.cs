using JNogueira.Bufunfa.Dominio;
using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar agendamentos
    public class ProcurarAgendamentoViewModel : ProcurarViewModel
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Data da início da parcela
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataInicioParcela { get; set; }

        /// <summary>
        /// Data da fim da parcela
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataFimParcela { get; set; }

        /// <summary>
        /// Procurar apenas por agendamentos concluídos
        /// </summary>
        public bool? Concluido { get; set; }

        public ProcurarAgendamentoViewModel()
        {
            this.OrdenarPor = "DataProximaParcelaAberta";
        }
    }
}
