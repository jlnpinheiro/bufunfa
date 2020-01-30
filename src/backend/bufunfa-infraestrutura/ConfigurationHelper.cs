using Microsoft.Extensions.Configuration;
using System;

namespace JNogueira.Bufunfa.Infraestrutura
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// Extrai a string de conexão com o banco de dados do sistema
        /// </summary>
        public string BancoDadosStringConnection => Configuration["BUFUNFA_BANCO_DADOS_CONNECTION_STRING"];

        /// <summary>
        /// Extrai a URL do webhook para envio de mensagens via ILogger para o Discord
        /// </summary>
        public string DiscordWebhookUrl => Configuration["BUFUNFA_DISCORD_WEBHOOK_URL"];

        /// <summary>
        /// Extrai o ID da pasta no Google Drive responsável pelo armazenamento dos anexos
        /// </summary>
        public string IdPastaGoogleDriveAnexos => Configuration["BUFUNFA_GOOGLE_DRIVE_ID_PASTA_ANEXO"];

        /// <summary>
        /// Extrai as informações de configuração para utilização do token JWT
        /// </summary>
        public JwtTokenConfig JwtTokenConfig { get; }

        /// <summary>
        /// Extrai as informações de configuração para utlizar a API da Alpha Vantage (consulta cotação de ativos)
        /// </summary>
        public ApiAlphaVantageConfig ApiAlphaVantageConfig { get; }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Classe para extração das informações do arquivo de configuração da aplicação ou das variáveis de ambiente
        /// </summary>
        public ConfigurationHelper(IConfiguration configuration)
        {
            Configuration = configuration;

            JwtTokenConfig = new JwtTokenConfig(configuration);

            ApiAlphaVantageConfig = new ApiAlphaVantageConfig(configuration);
        }
    }

    /// <summary>
    /// Classe que extrai as informações de configuração para utilização do token JWT
    /// </summary>
    public class JwtTokenConfig
    {
        public string Audience { get; }

        public string Issuer { get; }

        public int ExpiracaoEmHoras { get; }

        public string SecurityKey { get; set; }

        public JwtTokenConfig(IConfiguration configuration)
        {
            Audience = configuration["BUFUNFA_JWT_TOKEN_AUDIENCE"];

            Issuer = configuration["BUFUNFA_JWT_TOKEN_ISSUER"];

            ExpiracaoEmHoras = string.IsNullOrEmpty(configuration["BUFUNFA_JWT_TOKEN_TEMPO_EXPIRACAO_HORAS"])
                ? 8760
                : Convert.ToInt32(configuration["BUFUNFA_JWT_TOKEN_TEMPO_EXPIRACAO_HORAS"]);

            SecurityKey = string.IsNullOrEmpty(configuration["BUFUNFA_JWT_TOKEN_SECURITY_KEY"])
                ? "F7DEC0E0-4F78-4ED4-A89A-4EF3C6B82374"
                : configuration["BUFUNFA_JWT_TOKEN_SECURITY_KEY"];
        }
    }

    /// <summary>
    /// Classe que extrai as informações de configuração para utlizar a API da Alpha Vantage (consulta cotação de ativos)
    /// </summary>
    public class ApiAlphaVantageConfig
    {
        public string Key { get; }

        public string UrlGlobalQuotes { get; }

        public ApiAlphaVantageConfig(IConfiguration configuration)
        {
            Key = configuration["BUFUNFA_API_ALPHA_VANTAGE_KEY"];

            UrlGlobalQuotes = configuration["BUFUNFA_API_ALPHA_VANTAGE_URL_GLOBAL_QUOTES"];
        }
    }
}
