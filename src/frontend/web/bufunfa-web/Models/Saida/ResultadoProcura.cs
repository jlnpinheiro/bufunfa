using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    public class ResultadoProcura<TRegistro>
    {
        /// <summary>
        /// Indice da página
        /// </summary>
        public int? PaginaIndex { get; set; }

        /// <summary>
        /// Número de registros exibido na página
        /// </summary>
        public int? PaginaTamanho { get; set; }

        /// <summary>
        /// Nome da propriedade pela qual os dados serão ordenados
        /// </summary>
        public string OrdenarPor { get; set; }

        /// <summary>
        /// Sentido de ordenação dos dados
        /// </summary>
        public string OrdenarSentido { get; set; }

        /// <summary>
        /// Total de registros encontrados
        /// </summary>
        public int TotalRegistros { get; set; }

        /// <summary>
        /// Total de páginas encontradas
        /// </summary>
        public int? TotalPaginas { get; set; }

        /// <summary>
        /// Coleção de registros encontrados
        /// </summary>
        public IEnumerable<TRegistro> Registros { get; set; }
    }
}
