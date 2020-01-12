using JNogueira.Bufunfa.Dominio.Comandos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JNogueira.Bufunfa.Api.Filters
{
    /// <summary>
    /// Filtro que extrai as mensagens do ModelState e coloca no padrão de saida da API.
    /// </summary>
    public class CustomModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var validationResult = new ValidationResultModel(context.ModelState);

                context.Result = new JsonResult(new Saida(false, validationResult.Erros.Select(x => x.Mensagem + (!string.IsNullOrEmpty(x.Campo) ? $" ({x.Campo})" : string.Empty)).ToArray(), null));
            }
        }
    }

    public class ValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Campo { get; }

        public string Mensagem { get; }

        public ValidationError(string field, string message)
        {
            Campo = field != string.Empty ? field : null;
            Mensagem = message;
        }
    }

    public class ValidationResultModel
    {
        public List<ValidationError> Erros { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            Erros = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, string.IsNullOrEmpty(x.ErrorMessage) ? x.Exception?.Message : x.ErrorMessage)))
                    .ToList();
        }
    }
}
