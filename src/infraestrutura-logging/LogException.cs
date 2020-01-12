using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Infraestrutura.Logging
{
    internal class LogException
    {
        public LogExceptionInfo ExceptionInfo { get; }

        public LogExceptionRequest Request { get; }

        public LogException(Exception exception, IHttpContextAccessor httpContextAccessor)
        {
            if (exception == null)
                return;

            if (httpContextAccessor == null)
                return;

            var request = httpContextAccessor.HttpContext?.Request;

            if (request != null)
                this.Request = new LogExceptionRequest(request);

            this.ExceptionInfo = new LogExceptionInfo(exception);
        }
    }

    internal class LogExceptionRequest
    {
        public string Rota { get; }

        public Dictionary<string, string> Headers { get; }

        public LogExceptionRequest(HttpRequest request)
        {
            this.Headers = new Dictionary<string, string>();

            var uriBuilder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            };

            if (request.Host.Port.HasValue && request.Host.Port.Value != 80)
                uriBuilder.Port = request.Host.Port.Value;

            this.Rota = uriBuilder.Uri.ToString();

            foreach (var item in request.Headers.Where(x => x.Key != "Cookie" && x.Value.Any()))
                this.Headers.Add(item.Key, string.Join(",", item.Value.ToArray()));
        }
    }

    internal class LogExceptionInfo
    {
        public string ExceptionMensagem { get; }

        public string BaseExceptionMensagem { get; }

        public string StackTrace { get; }

        public string Source { get; }

        public LogExceptionInfo(Exception exception)
        {
            this.ExceptionMensagem = exception.Message;

            this.BaseExceptionMensagem = exception.GetBaseException().Message;

            this.StackTrace = exception.StackTrace;

            this.Source = exception.Source;
        }
    }
}
