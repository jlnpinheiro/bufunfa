using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace JNogueira.Bufunfa.Web.Helpers
{
    /// <summary>
    /// Classe para extração dos parâmetros utilizados pelo componente Datatables.net
    /// </summary>
    public class DatatablesHelper
    {
        private readonly HttpContext _httpContext;

        public DatatablesHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }

        public string PalavraChave
        {
            get { return _httpContext.Request.Form["search[value]"].FirstOrDefault(); }
        }

        public int Draw
        {
            get
            {
                return !_httpContext.Request.HasFormContentType || !_httpContext.Request.Form["draw"].Any()
              ? -1
              : Convert.ToInt32(_httpContext.Request.Form["draw"].First());
            }
        }

        public int PaginaIndex
        {
            get
            {
                if (string.IsNullOrEmpty(_httpContext.Request.Form["length"].FirstOrDefault())
                    || string.IsNullOrEmpty(_httpContext.Request.Form["start"].FirstOrDefault()))
                    return -1;

                var length = Convert.ToInt32(_httpContext.Request.Form["length"].FirstOrDefault());
                var start = Convert.ToInt32(_httpContext.Request.Form["start"].FirstOrDefault());

                return (start / length) + 1;
            }
        }

        public int PaginaTamanho
        {
            get
            {
                return !_httpContext.Request.Form["length"].Any()
              ? 1
              : Convert.ToInt32(_httpContext.Request.Form["length"].FirstOrDefault());
            }
        }

        public string OrdenarSentido
        {
            get { return _httpContext.Request.Form["order[0][dir]"].FirstOrDefault() ?? string.Empty; }
        }

        public string OrdenarPor
        {
            get
            {
                var colunaOrdenacaoIndex = -1;

                return int.TryParse(_httpContext.Request.Form["order[0][column]"].FirstOrDefault(), out colunaOrdenacaoIndex)
                    ? _httpContext.Request.Form["columns[" + colunaOrdenacaoIndex + "][data]"].FirstOrDefault()
                    : string.Empty;
            }
        }
    }
}
