using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Helpers
{
    public class HttpClientHelper
    {
        private readonly CookieHelper _cookieHelper;
        private readonly string _urlApi;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpClientHelper(ConfigurationHelper configHelper, CookieHelper cookieHelper, IHttpClientFactory httpClientFactory)
        {
            _cookieHelper = cookieHelper;

            _urlApi = configHelper.UrlBackend;

            _httpClientFactory = httpClientFactory;
        }

        public async Task<HttpResponseMessage> FazerRequest(string rota, MetodoHttp metodo, HttpContent httpContent = null, bool usarJwtToken = true)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.Timeout = new TimeSpan(0, 0, 30);

                if (usarJwtToken)
                {
                    var tokenJwt = _cookieHelper.ObterTokenJwt();

                    if (string.IsNullOrEmpty(tokenJwt))
                        throw new NullReferenceException("Não foi possível extrair o token JWT do cookie de autenticação.");

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenJwt);
                }

                switch (metodo)
                {
                    case MetodoHttp.GET:
                        return await client.GetAsync(_urlApi + rota);
                    case MetodoHttp.POST:
                        return await client.PostAsync(_urlApi + rota, httpContent);
                    case MetodoHttp.PUT:
                        return await client.PutAsync(_urlApi + rota, httpContent);
                    case MetodoHttp.DELETE:
                        return await client.DeleteAsync(_urlApi + rota);
                    default:
                        throw new NotImplementedException("Método não atendido pela classe HttpClientHelper.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha no acesso a URL {_urlApi + rota}: {ex.Message}", ex);
            }
        }

        public async Task<TSaida> FazerRequest<TSaida>(string rota, MetodoHttp metodo, HttpContent httpContent = null, bool usarJwtToken = true)
        {
            var response = await FazerRequest(rota, metodo, httpContent, usarJwtToken);

            return await response.Content.ReadAsAsync<TSaida>();
        }
    }

    public enum MetodoHttp
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}
