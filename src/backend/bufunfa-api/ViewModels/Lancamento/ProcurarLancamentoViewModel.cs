using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar lançamentos
    public class ProcurarLancamentoViewModel
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Data da início do lançamento
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Data do fim do lançamento
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataFim { get; set; }

        [EnumDataType(typeof(LancamentoOrdenarPor))]
        [JsonConverter(typeof(StringEnumConverter))]
        public LancamentoOrdenarPor OrdenarPor { get; set; }

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
