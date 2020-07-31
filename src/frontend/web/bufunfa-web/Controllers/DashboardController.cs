using JNogueira.Bufunfa.Web.Binders;
using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("dashboard")]
    public class DashboardController : BaseController
    {
        public DashboardController(BackendProxy proxy)
            : base(proxy)
        {
        }

        [HttpGet]
        [ExibirPeriodoAtualFilter]
        [ExibirAtalhosFilter]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("listar-parcelas-para-timeline")]
        public async Task<IActionResult> ListarParcelasParaTimeline([DateTimeModelBinder(DateFormat = "dd/MM/yyyy")] DateTime? dataInicio, [DateTimeModelBinder(DateFormat = "dd/MM/yyyy")] DateTime? dataFim, bool somenteParcelasAbertas = true)
        {
            var saida = await _proxy.ObterParcelasPorPeriodo(dataInicio.Value, dataFim.Value, somenteParcelasAbertas);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, $"Não foi possível obter as parcelas do período {dataInicio.Value.ToString("dd/MM/yyyy")} até {dataFim.Value.ToString("dd/MM/yyyy")}.", saida.Mensagens));

            return PartialView("ListarParcelasParaTimeline", saida.Retorno);
        }

        [HttpGet]
        [Route("listar-contas")]
        public async Task<IActionResult> ListarContas()
        {
            var saida = await _proxy.ObterContas();

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as contas.", saida.Mensagens));

            return PartialView("ListarContas", saida.Retorno?.Where(x => x.CodigoTipo != (int)TipoConta.Acoes && x.CodigoTipo != (int)TipoConta.FII && x.ValorSaldoAtual != 0));
        }

        [HttpGet]
        [Route("listar-cartoes")]
        public async Task<IActionResult> ListarCartoes()
        {
            var saida = await _proxy.ObterCartoesCredito();

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter os cartões de crédito.", saida.Mensagens));

            return PartialView("ListarCartoes", saida.Retorno);
        }

        [HttpGet]
        [Route("listar-acoes")]
        public async Task<IActionResult> ListarAcoes()
        {
            var saida = await _proxy.ObterAnaliseAtivos();

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as ações.", saida.Mensagens));

            return PartialView("ListarAcoes", saida.Retorno.Where(x => x.ValorTotalCompra > 0));
        }
    }
}
