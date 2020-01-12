using JNogueira.Infraestrutura.Utilzao.Slack;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Infraestrutura.Logging.Slack
{
    public class SlackLogger : ILogger
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SlackUtil _slackUtil;
        private readonly string _channel;
        private readonly string _modulo;
        private readonly string _nomeEnvironment;
        private readonly string _nomeFuncionalidade;
        private readonly string _userName;

        public SlackLogger(
            string nomeFuncionalidade,
            string webHookUrl,
            string channel,
            IHttpContextAccessor httpContextAccessor,
            string nomeEnvironment,
            string modulo,
            string userName = null)
        {
            _channel             = channel;
            _httpContextAccessor = httpContextAccessor;
            _modulo              = modulo;
            _nomeEnvironment     = nomeEnvironment;
            _nomeFuncionalidade  = nomeFuncionalidade;
            _slackUtil           = new SlackUtil(webHookUrl);
            _userName            = userName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (formatter == null)
                throw new ArgumentNullException(nameof(formatter));

            var mensagem = formatter(state, exception);

            if (string.IsNullOrEmpty(mensagem))
                return;

            var slackMensagem = new SlackMensagem(_channel, mensagem, _userName);

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Information:
                    slackMensagem.Titulo = logLevel == LogLevel.Information ? "Info" : logLevel.ToString();
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Info);
                    break;
                case LogLevel.Warning:
                    slackMensagem.Titulo = "Atenção";
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Aviso);
                    break;
                case LogLevel.Error:
                    slackMensagem.Titulo = "Erro";
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Erro);
                    break;
                case LogLevel.Critical:
                    slackMensagem.Titulo = logLevel.ToString();
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Aviso);
                    break;
                case LogLevel.None:
                    slackMensagem.DefinirTipo(TipoSlackMensagem.Info);
                    break;
            }

            var slackInfoAdicionais = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("Ambiente", _nomeEnvironment),
                new KeyValuePair<string, string>("Modulo", _modulo),
                new KeyValuePair<string, string>("Funcionalidade", _nomeFuncionalidade)
            };

            var nomeUsuario = _httpContextAccessor.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "Nome")?.Value;

            if (!string.IsNullOrEmpty(nomeUsuario))
                slackInfoAdicionais.Add(new KeyValuePair<string, string>("Usuário", nomeUsuario));

            if (exception != null)
            {
                var logExcpetion = new LogException(exception, _httpContextAccessor);

                if (logExcpetion.Request != null)
                {
                    slackInfoAdicionais.Add(new KeyValuePair<string, string>("Rota", logExcpetion.Request.Rota));

                    if (logExcpetion.Request.Headers != null && logExcpetion.Request.Headers.Any())
                        slackInfoAdicionais.Add(new KeyValuePair<string, string>("Request Header", JsonConvert.SerializeObject(logExcpetion.Request.Headers, Formatting.Indented)));
                }

                foreach (KeyValuePair<string, string> data in exception.Data)
                    slackInfoAdicionais.Add(new KeyValuePair<string, string>(data.Key, data.Value));

                _slackUtil.Postar(slackMensagem, exception, slackInfoAdicionais);
            }
            else
            {
                _slackUtil.Postar(slackMensagem, infoAdicionais: slackInfoAdicionais);
            }
        }
    }
}
