using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de uma parcela de agendamento
    /// </summary>
    public class ManterParcela : BaseModel
    {
        /// <summary>
        /// Id da parcela
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int IdAgendamento { get; set; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; set; }
    }
}
