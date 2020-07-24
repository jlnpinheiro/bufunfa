using JNogueira.Bufunfa.Infraestrutura.Integracoes.AlphaVantage.Comandos;
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
        private readonly ILogger<ApiAlphaVantageProxy> _logger;
        private readonly ConfigurationHelper _configHelper;

        public ApiAlphaVantageProxy(ConfigurationHelper configHelper, ILogger<ApiAlphaVantageProxy> logger)
        {
            _logger       = logger;
            _configHelper = configHelper;
        }

        public async Task<GlobalQuoteSaida> ObterCotacaoPorSiglaAtivo(string sigla)
        {
            if (string.IsNullOrEmpty(sigla) || string.IsNullOrEmpty(_configHelper.ApiAlphaVantageConfig.UrlGlobalQuotes) || string.IsNullOrEmpty(_configHelper.ApiAlphaVantageConfig.Key))
                return null;

            try
            {
                using (var client = new HttpClient { Timeout = new TimeSpan(0, 0, 15) })
                {
                    var response = await client.GetAsync(string.Format(_configHelper.ApiAlphaVantageConfig.UrlGlobalQuotes, sigla.ToUpper(), _configHelper.ApiAlphaVantageConfig.Key));

                    var globalQuote = GlobalQuote.Obter(response.Content.ReadAsStringAsync().Result);

                    return globalQuote.Params != null
                        ? new GlobalQuoteSaida(globalQuote.Params)
                        : null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Erro ao utilizar a query \"GLOBAL_QUOTE\" ({string.Format(_configHelper.ApiAlphaVantageConfig.UrlGlobalQuotes, sigla, _configHelper.ApiAlphaVantageConfig.Key)}) para a sigla {sigla} da API da Alpha Vantage.");

                return null;
            }
        }
    }
}
