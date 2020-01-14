using JNogueira.Bufunfa.Web.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace JNogueira.Bufunfa.Web.Filters
{
    /// <summary>
    /// Filtro para ser utilizado nas views que exibem os atalhos do usuário
    /// </summary>
    public class ExibirAtalhosFilterAttribute : TypeFilterAttribute
    {
        public ExibirAtalhosFilterAttribute() 
            : base(typeof(ExibirAtalhosFilter))
        {

        }

        private class ExibirAtalhosFilter : ActionFilterAttribute
        {
            private readonly BackendProxy _proxy;

            public ExibirAtalhosFilter(BackendProxy proxy)
            {
                _proxy = proxy;
            }

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                base.OnActionExecuting(context);

                var controller = context.Controller as Controller;

                if (controller == null) return;

                var saida = _proxy.ObterAtalhos().Result;

                if (saida.Sucesso)
                    controller.ViewBag.Atalhos = saida.Retorno;
            }
        }
    }

    
}
