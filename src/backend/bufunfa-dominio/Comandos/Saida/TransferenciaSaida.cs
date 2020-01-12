using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de uma transferência de valore entre contas
    /// </summary>
    public class TransferenciaSaida
    {
        /// <summary>
        /// Conta origem da transferência
        /// </summary>
        public ContaSaida ContaOrigem { get; }

        /// <summary>
        /// Conta destino da transferência
        /// </summary>
        public ContaSaida ContaDestino { get; }

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

        public TransferenciaSaida(ContaSaida contaOrigem, ContaSaida contaDestino, DateTime data, decimal valor, string observacao)
        {
            this.ContaOrigem  = contaOrigem;
            this.ContaDestino = contaDestino;
            this.Data         = data;
            this.Valor        = valor;
            this.Observacao   = observacao;
        }
    }
}
