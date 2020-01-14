using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para a procura de períodos
    /// </summary>
    public class ProcurarPeriodo : BaseProcurar
    {
        /// <summary>
        /// Nome do período
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Data abrangida pelo período
        /// </summary>
        public DateTime? Data { get; set; }
    }
}
