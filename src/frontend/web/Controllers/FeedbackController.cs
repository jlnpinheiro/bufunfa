using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace JNogueira.Bufunfa.Web.Controllers
{
    public class FeedbackController : Controller
    {
        [AllowAnonymous]
        [Route("feedback/{httpStatusCode:int}")]
        public IActionResult Feedback(HttpStatusCode httpStatusCode)
        {
            Feedback feedback;

            switch (httpStatusCode)
            {
                case HttpStatusCode.NotFound:
                case HttpStatusCode.BadRequest:
                    var feature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
                    feedback = new Feedback(TipoFeedback.Atencao, $"A rota \"{feature?.OriginalPath}\" não foi encontrada.");
                    break;
                case HttpStatusCode.Forbidden:
                    feedback = new Feedback(TipoFeedback.Atencao, "Você não tem permissão para acessar essa funcionalidade.", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaInicial);
                    break;
                case HttpStatusCode.InternalServerError:
                    feedback = new Feedback(TipoFeedback.Erro, "Ooops! Um erro inesperado aconteceu...", new[] { "A ocorrência desse erro foi registrada e será posteriormente analisada para identificar a causa. Pedimos desculpas pelo transtorno." });
                    break;
                case HttpStatusCode.Unauthorized:
                    feedback = new Feedback(TipoFeedback.Atencao, "Você precisa realizar seu login antes de acessar essa funcionalidade.", tipoAcao: TipoAcaoAoOcultarFeedback.RedirecionarTelaLogin);
                    break;
                default:
                    feedback = new Feedback(TipoFeedback.Atencao, "Não foi possível acessar a página ou funcionalidade.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar);
                    break;
            }

            return new FeedbackResult(feedback);
        }
    }
}