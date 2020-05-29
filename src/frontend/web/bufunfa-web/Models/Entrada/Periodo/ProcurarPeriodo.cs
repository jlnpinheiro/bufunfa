using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para a procura de períodos
    /// </summary>
    public class ProcurarPeriodo : BaseProcurar<PeriodoOrdenarPor>
    {
        /// <summary>
        /// Nome do período
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data abrangida pelo período
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? Data { get; set; }
    }

    public enum PeriodoOrdenarPor
    {
        Nome,
        DataInicio,
        DataFim
    }
}
