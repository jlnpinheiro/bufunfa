using Newtonsoft.Json;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe para padronização das saídas da API
    /// </summary>
    public class Saida<TRetorno> : BaseModel
    {
        /// <summary>
        /// Indica se houve sucesso
        /// </summary>
        [JsonProperty("sucesso")]
        public bool Sucesso { get; private set; }

        /// <summary>
        /// Mensagens retornadas
        /// </summary>
        [JsonProperty("mensagens")]
        public IEnumerable<string> Mensagens { get; private set; }

        /// <summary>
        /// Objeto retornado
        /// </summary>
        [JsonProperty("retorno")]
        public TRetorno Retorno { get; private set; }

        [JsonConstructor]
        private Saida()
        {

        }
    }

    public class Saida : BaseModel
    {
        /// <summary>
        /// Indica se houve sucesso
        /// </summary>
        [JsonProperty("sucesso")]
        public bool Sucesso { get; private set; }

        /// <summary>
        /// Mensagens retornadas
        /// </summary>
        [JsonProperty("mensagens")]
        public IEnumerable<string> Mensagens { get; private set; }

        /// <summary>
        /// Objeto retornado
        /// </summary>
        [JsonProperty("retorno")]
        public object Retorno { get; private set; }

        [JsonConstructor]
        private Saida()
        {

        }
    }
}
