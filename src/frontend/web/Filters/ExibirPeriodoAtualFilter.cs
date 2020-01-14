using JNogueira.Bufunfa.Web.Proxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace JNogueira.Bufunfa.Web.Filters
{
    /// <summary>
    /// Filtro para ser utilizado nas views que exibem o período que abrange a data atual
    /// </summary>
    public class ExibirPeriodoAtualFilterAttribute : TypeFilterAttribute
    {
        public ExibirPeriodoAtualFilterAttribute() 
            : base(typeof(ExibirPeriodoAtualFilter))
        {

        }

        private class ExibirPeriodoAtualFilter : ActionFilterAttribute
        {
            private readonly BackendProxy _proxy;

            public ExibirPeriodoAtualFilter(BackendProxy proxy)
            {
                _proxy = proxy;
            }

            public override void OnActionExecuting(ActionExecutingContext context)
            {
                base.OnActionExecuting(context);

                var controller = context.Controller as Controller;

                if (controller == null) return;

                var saida = _proxy.ObterPeriodoPorDataReferencia(DateTime.Now).Result;

                if (saida.Sucesso)
                    controller.ViewBag.PeriodoAtual = saida.Retorno;
            }
        }
    }

    
}
