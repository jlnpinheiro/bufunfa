using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para as informações de uma transferência de valore entre contas
    /// </summary>
    public class Transferencia
    {
        /// <summary>
        /// Conta origem da transferência
        /// </summary>
        public Conta ContaOrigem { get; }

        /// <summary>
        /// Conta destino da transferência
        /// </summary>
        public Conta ContaDestino { get; }

        /// <summary>
        /// Data da transferência
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; }

        /// <summary>
        /// Valor da transferência
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observações referentes a transferência
        /// </summary>
        public string Observacao { get; }
    }
}
