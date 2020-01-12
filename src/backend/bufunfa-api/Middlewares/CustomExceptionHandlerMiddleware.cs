using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Api.Middlewares
{
    /// <summary>
    /// Middleware para fazer o handler de erros HTTP ou de exceptions e padronizar o retorno.
    /// </summary>
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        private readonly ILogger _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);

                if (context.Response.StatusCode >= 200 && context.Response.StatusCode <= 299)
                    return;

                switch (context.Response.StatusCode)
                {
                    case (int)HttpStatusCode.Unauthorized:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new UnauthorizedApiResponse()));
                        break;
                    case (int)HttpStatusCode.Forbidden:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new ForbiddenApiResponse()));
                        break;
                    case (int)HttpStatusCode.NotFound:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new NotFoundApiResponse(context.Request.Path)));
                        break;
                    case (int)HttpStatusCode.UnsupportedMediaType:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new UnsupportedMediaTypeApiResponse(context.Request.ContentType)));
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.GetBaseException().Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                await context.Response.WriteAsync(JsonConvert.SerializeObject(new InternalServerErrorApiResponse(ex), new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
            }
        }
    }

    public static class CustomExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder) => builder.UseMiddleware(typeof(CustomExceptionHandlerMiddleware));
    }
}
