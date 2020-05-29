using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar agendamentos
    public class ProcurarAgendamentoViewModel
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

        [EnumDataType(typeof(AgendamentoOrdenarPor))]
        [JsonConverter(typeof(StringEnumConverter))]
        public AgendamentoOrdenarPor OrdenarPor { get; set; }

        /// <summary>
        /// Sentido da ordenação que será utilizado ordernar os registros encontrados (ASC para crescente; DESC para decrescente)
        /// </summary>
        public string OrdenarSentido { get; set; }

        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; set; }
    }
}
