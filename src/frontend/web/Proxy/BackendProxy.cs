using JNogueira.Bufunfa.Web.Helpers;

namespace JNogueira.Bufunfa.Web.Proxy
{
    /// <summary>
    /// Classe proxy com os métodos para acesso ao backend
    /// </summary>
    public partial class BackendProxy
    {
        private readonly HttpClientHelper _httpClientHelper;

        public BackendProxy(HttpClientHelper httpClientHelper)
        {
            _httpClientHelper = httpClientHelper;
        }
    }
}
