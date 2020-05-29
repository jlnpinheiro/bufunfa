using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para a procura de lançamentos
    /// </summary>
    public class ProcurarLancamento : BaseProcurar<LancamentoOrdenarPor>
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public int IdConta { get; set; }

        /// <summary>
        /// ID da categoria
        /// </summary>
        public int? IdCategoria { get; set; }

        /// <summary>
        /// ID da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Data de início do período para procura
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data fim do período para procura
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime DataFim { get; set; }
    }

    public enum LancamentoOrdenarPor
    {
        CategoriaCaminho,
        NomePessoa,
        NomeConta,
        Valor,
        Data
    }
}
