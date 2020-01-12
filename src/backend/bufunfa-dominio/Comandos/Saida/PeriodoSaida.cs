using JNogueira.Bufunfa.Dominio.Entidades;
using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de um período
    /// </summary>
    public class PeriodoSaida
    {
        /// <summary>
        /// ID do período
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome do período
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Data início do período
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime DataInicio { get; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime DataFim { get; }

        /// <summary>
        /// Quantidade de dias do período
        /// </summary>
        public double QuantidadeDias =>  Math.Round((this.DataFim - this.DataInicio).TotalDays, 0);

        public PeriodoSaida(Periodo periodo)
        {
            if (periodo == null)
                return;

            this.Id         = periodo.Id;
            this.Nome       = periodo.Nome;
            this.DataInicio = periodo.DataInicio;
            this.DataFim    = periodo.DataFim;
        }

        public PeriodoSaida(int id, string nome, DateTime dataInicio, DateTime dataFim)
        {
            Id = id;
            Nome = nome;
            DataInicio = dataInicio;
            DataFim = dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        public override string ToString()
        {
            return $"{this.Nome} - {this.DataInicio.ToString("dd/MM/yyyy")} até {this.DataFim.ToString("dd/MM/yyyy")}";
        }
    }
}
