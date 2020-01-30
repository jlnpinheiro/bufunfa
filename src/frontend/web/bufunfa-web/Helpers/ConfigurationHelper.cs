using Microsoft.Extensions.Configuration;

namespace JNogueira.Bufunfa.Web.Helpers
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// Extrai a string de conexão com o banco de dados do sistema
        /// </summary>
        public string UrlBackend => Configuration["BUFUNFA_BACKEND_URL"];

        /// <summary>
        /// Extrai a URL do webhook para envio de mensagens via ILogger para o Discord
        /// </summary>
        public string DiscordWebhookUrl => Configuration["BUFUNFA_DISCORD_WEBHOOK_URL"];

        public IConfiguration Configuration { get; }

        /// <summary>
        /// Classe para extração das informações do arquivo de configuração da aplicação ou das variáveis de ambiente
        /// </summary>
        public ConfigurationHelper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
