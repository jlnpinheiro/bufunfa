using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada referente a um transferência entre contas
    /// </summary>
    public class Transferir : BaseModel
    {
        /// <summary>
        /// ID da conta de origem
        /// </summary>
        public int IdContaOrigem { get; set; }

        /// <summary>
        /// ID da conta de destino
        /// </summary>
        public int IdContaDestino { get; set; }

        /// <summary>
        /// Data da transferência
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor da transferência
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Observações referentes a transferência
        /// </summary>
        public string Observacao { get; set; }
    }
}
