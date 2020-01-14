using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de um período
    /// </summary>
    public class Periodo
    {
        /// <summary>
        /// ID do período
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do período
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data inicial do período
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data final do período
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime DataFim { get; set; }

        /// <summary>
        /// Quantidade de dias abrangidos pelo período
        /// </summary>
        public double QuantidadeDias { get; set; }

        public double QuantidadeDiasFimPeriodo => Math.Round(this.DataFim.Subtract(DateTime.Now).TotalDays, 0);

        public double ObterPercentualConclusao()
        {
            return Math.Round(this.QuantidadeDiasFimPeriodo * 100 / this.QuantidadeDias, 0);
        }
    }
}
