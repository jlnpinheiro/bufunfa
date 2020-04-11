using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("graficos")]
    public class GraficoController : BaseController
    {
        public GraficoController(BackendProxy proxy)
            : base(proxy)
        {
            
        }

        [HttpGet]
        [Route("exibir-parametros-valor-por-categoria")]
        public IActionResult ExibirPopupParametrosValorPorCategoria()
        {
            return PartialView("PopupParametrosValorPorCategoria");
        }

        [HttpGet]
        [Route("exibir-popup-grafico-valor-por-categoria")]
        public IActionResult ExibirPopupGraficoValorPorCategoria(int idCategoria, int ano)
        {
            ViewData["IdCategoria"] = idCategoria;
            ViewData["Ano"] = ano;

            return PartialView("PopupGraficoValorPorCategoria");
        }

        [HttpGet]
        [Route("ober-dados-grafico-valor-por-categoria")]
        public async Task<IActionResult> ObterDadosGraficoValorPorCategoria(int idCategoria, int ano)
        {
            var saida = await _proxy.GerarGraficoRelacaoValorCategoriaPorAno(ano, idCategoria);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível obter as informações para geração do gráfico.", saida.Mensagens))
                : (IActionResult)Json(saida.Retorno);
        }

        [HttpGet]
        [Route("exibir-popup-periodo-grafico-valor-por-categoria")]
        public async Task<IActionResult> ExibirPopupPeriodoGraficoValorPorCategoria(int idPeriodo, int idCategoria, int ano)
        {
            var saida = await _proxy.ObterPeriodoGraficoRelacaoValorCategoriaPorAno(idPeriodo, ano, idCategoria);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível obter as informações para geração do gráfico.", saida.Mensagens))
                : (IActionResult)PartialView("PopupDetalhePeriodoGraficoValorPorCategoria", saida.Retorno);
        }
    }
}
