using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace JNogueira.Bufunfa.Infraestrutura.Logging.Slack
{
    public class SlackLoggerProvider : ILoggerProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _channel;
        private readonly string _modulo;
        private readonly string _nomeEnvironment;
        private readonly string _userName;
        private readonly string _webHookUrl;
        private SlackLogger _logger;

        public SlackLoggerProvider(string webHookUrl, string channel, IHttpContextAccessor httpContextAccessor, string nomeEnvironment, string modulo, string userName = null)
        {
            _channel             = channel;
            _httpContextAccessor = httpContextAccessor;
            _modulo              = modulo;
            _nomeEnvironment     = nomeEnvironment;
            _userName            = userName;
            _webHookUrl          = webHookUrl;
        }

        public ILogger CreateLogger(string categoryName)
        {
            _logger = new SlackLogger(categoryName, _webHookUrl, _channel, _httpContextAccessor, _nomeEnvironment, _modulo, _userName);

            return _logger;
        }

        public void Dispose()
        {
            _logger = null;
        }
    }

    public static class SlackLoggerProviderExtensions
    {
        public static ILoggerFactory AddSlackLoggerProvider(this ILoggerFactory loggerFactory, string webHookUrl, string channel, IHttpContextAccessor httpContextAccessor, string nomeEnvironment, string modulo, string userName = null)
        {
            loggerFactory.AddProvider(new SlackLoggerProvider(webHookUrl, channel, httpContextAccessor, nomeEnvironment, modulo, userName));
            return loggerFactory;
        }
    }
}
