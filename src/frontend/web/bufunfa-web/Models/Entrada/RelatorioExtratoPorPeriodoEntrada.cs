using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para exibição o extrato por período
    /// </summary>
    public class RelatorioExtratoPorPeriodoEntrada : BaseModel
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public int IdConta { get; set; }

        /// <summary>
        /// ID do período
        /// </summary>
        public int? IdPeriodo { get; set; }

        /// <summary>
        /// Data de início do período
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        public DateTime DataFim { get; set; }

        public bool GerarPdf { get; set; }

        public RelatorioExtratoPorPeriodoEntrada()
        {
            GerarPdf = false;
        }
    }
}
