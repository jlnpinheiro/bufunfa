using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de um período
    /// </summary>
    public class ManterPeriodo : BaseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome do período
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data inicial do período
        /// </summary>
        public DateTime DataInicio { get; set; }

        /// <summary>
        /// Data final do período
        /// </summary>
        public DateTime DataFim { get; set; }
    }
}
