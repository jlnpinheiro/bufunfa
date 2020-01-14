using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace JNogueira.Bufunfa.Web.Filters
{
    public class FeedbackExceptionFilterAttribute : TypeFilterAttribute
    {
        public FeedbackExceptionFilterAttribute(string mensagem, TipoAcaoAoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional = "") 
            : base(typeof(FeedbackExceptionFilter))
        {
            Arguments = new object[] { mensagem, tipoAcaoOcultar, mensagemAdicional };
        }

        private class FeedbackExceptionFilter : ExceptionFilterAttribute
        {
            private readonly IWebHostEnvironment _hostEnvironment;
            private readonly ILoggerFactory _loggerFactory;

            private readonly string _mensagem;
            private readonly TipoAcaoAoOcultarFeedback _tipoAcaoOcultar;
            private readonly string _mensagemAdicional;

            public FeedbackExceptionFilter(IWebHostEnvironment hostEnvironment, ILoggerFactory loggerFactory, string mensagem, TipoAcaoAoOcultarFeedback tipoAcaoOcultar, string mensagemAdicional)
            {
                _hostEnvironment = hostEnvironment;

                _mensagem = mensagem;
                _tipoAcaoOcultar = tipoAcaoOcultar;
                _mensagemAdicional = !string.IsNullOrEmpty(mensagemAdicional) ? mensagemAdicional : "A ocorrência desse erro foi registrada e será posteriormente analisada para identificar a causa. Pedimos desculpas pelo transtorno.";
                _loggerFactory = loggerFactory;
            }

            public override void OnException(ExceptionContext context)
            {
                HandleException(context);

                base.OnException(context);
            }

            private void HandleException(ExceptionContext context)
            {
                var feedback = new Feedback(TipoFeedback.Erro, _mensagem, new[] { _mensagemAdicional  }, _tipoAcaoOcultar);

                var logger = _loggerFactory.CreateLogger<FeedbackExceptionFilter>();

                logger.LogError(context.Exception, _mensagem);

                context.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                context.Result = new FeedbackResult(feedback);
            }
        }
    }
}