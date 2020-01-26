using Newtonsoft.Json;

namespace JNogueira.Bufunfa.Web.Models
{
    public abstract class BaseModel
    {
        /// <summary>
        /// Obtém a representação do objeto em JSON
        /// </summary>
        public string ObterJson() => this == null
            ? string.Empty
            : JsonConvert.SerializeObject(this);
    }
}
