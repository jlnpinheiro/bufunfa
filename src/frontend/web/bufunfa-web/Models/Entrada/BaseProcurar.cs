using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe ancestral que contém todas as propriedades comuns relacionadas a procura
    /// </summary>
    public abstract class BaseProcurar<TOrdenarPor> : BaseModel
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
        [JsonConverter(typeof(StringEnumConverter))]
        public TOrdenarPor OrdenarPor { get; set; }

        /// <summary>
        /// Sentido de ordenação dos dados
        /// </summary>
        public string OrdenarSentido { get; set; }
    }
}
