using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Filters
{
    /// <summary>
    /// ActionFilterAttribute que extrai as mensagens de validação do ModelState e transeforma em um feedback
    /// </summary>
    public class CustomModelStateValidationFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var lstErros = new List<string>();

                foreach (var modelState in context.ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        lstErros.Add(error.Exception != null ? error.Exception.Message : error.ErrorMessage);
                    }
                }

                context.Result = new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Por favor corrija os erros abaixo:", lstErros));
            }
        }
    }
}
