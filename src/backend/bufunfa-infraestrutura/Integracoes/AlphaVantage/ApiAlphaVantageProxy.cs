using JNogueira.Bufunfa.Infraestrutura.Integracoes.AlphaVantage.Comandos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Integracoes.AlphaVantage
{
    /// <summary>
    /// Proxy para a API que possibilita a consulta da cotação de uma ação
    /// </summary>
    public class ApiAlphaVantageProxy
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<ApiAlphaVantageProxy> _logger;
        private readonly string _apiKey;

        public ApiAlphaVantageProxy(IConfiguration configuration, ILogger<ApiAlphaVantageProxy> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _apiKey = configuration["ApiAlphaVantage:Key"];
        }

        public async Task<GlobalQuoteSaida> ObterCotacaoPorSiglaAcao(string sigla)
        {
            if (string.IsNullOrEmpty(sigla))
                return null;

            try
            {
                using (var client = new HttpClient { Timeout = new TimeSpan(0, 0, 15) })
                {
                    var response = await client.GetAsync(string.Format(_configuration["ApiAlphaVantage:UrlGlobalQuotes"], sigla.ToUpper(), _apiKey));

                    var globalQuote = GlobalQuote.Obter(response.Content.ReadAsStringAsync().Result);

                    return globalQuote.Params != null
                        ? new GlobalQuoteSaida(globalQuote.Params)
                        : null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao utilizar a query \"GLOBAL_QUOTE\" ({string.Format(_configuration["ApiAlphaVantage:UrlGlobalQuotes"], sigla, _apiKey)}) para a sigla {sigla} da API da Alpha Vantage.");

                return null;
            }
        }
    }
}
