using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar períodos
    public class ProcurarPeriodoViewModel
    {
        /// <summary>
        /// Nome da período
        /// </summary>
        [MaxLength(50, ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// Data abrangida pela data início e fim do périodo (deve ser superior á 01/01/2015).
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? Data { get; set; }

        [EnumDataType(typeof(PeriodoOrdenarPor))]
        [JsonConverter(typeof(StringEnumConverter))]
        public PeriodoOrdenarPor OrdenarPor { get; set; }

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
