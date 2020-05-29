using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar a
    public class ProcurarPessoaViewModel
    {
        /// <summary>
        /// Nome da pessoa
        /// </summary>
        [MaxLength(200, ErrorMessageResourceType = typeof(PessoaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        [EnumDataType(typeof(PessoaOrdenarPor))]
        [JsonConverter(typeof(StringEnumConverter))]
        public PessoaOrdenarPor OrdenarPor { get; set; }

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
