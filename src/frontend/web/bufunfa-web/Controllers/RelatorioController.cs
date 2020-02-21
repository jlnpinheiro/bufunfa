using JNogueira.Bufunfa.Web.Binders;
using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Models;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using System;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("relatorios")]
    public class RelatorioController : BaseController
    {
        public RelatorioController(BackendProxy proxy)
            : base(proxy)
        {
        }

        [HttpGet]
        [ExibirPeriodoAtualFilter]
        [Route("exibir-parametros-extrato-por-periodo")]
        public IActionResult ObterPopupExtratoPorPeriodo()
        {
            return PartialView("PopUpParametrosExtratoPorPeriodo");
        }

        [HttpGet]
        [Route("gerar-extrato-por-periodo")]
        public async Task<IActionResult> GerarExtratoPorPeriodo(
            [DateTimeModelBinder(DateFormat = "dd/MM/yyyy")] DateTime? dataInicio,
            [DateTimeModelBinder(DateFormat = "dd/MM/yyyy")] DateTime? dataFim,
            int idConta,
            int? idPeriodo = null,
            bool gerarPdf = false)
        {
            var contaSaida = await _proxy.ObterContaPorId(idConta);

            if (!contaSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da conta.", contaSaida.Mensagens));

            Periodo periodo = null;

            if (idPeriodo.HasValue)
            {
                var periodoSaida = await _proxy.ObterPeriodoPorId(idPeriodo.Value);

                if (!periodoSaida.Sucesso)
                    return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações do período.", periodoSaida.Mensagens));

                periodo = periodoSaida.Retorno;

                dataInicio = periodo.DataInicio;
                dataFim = periodo.DataFim;
            }

            var saida = new RelatorioExtratoPorPeriodoSaida { Conta = contaSaida.Retorno };

            if (periodo != null)
            {
                saida.Periodo    = periodo;
                saida.DataInicio = periodo.DataInicio;
                saida.DataFim    = periodo.DataFim;
            }
            else
            {
                saida.DataInicio = dataInicio.Value;
                saida.DataFim    = dataFim.Value;
            }

            var lancamentosSaida = await _proxy.ProcurarLancamentos(new ProcurarLancamento { IdConta = idConta, DataInicio = dataInicio.Value, DataFim = dataFim.Value });

            if (!lancamentosSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter os lançamento do período informado para a conta.", lancamentosSaida.Mensagens));

            saida.Lancamentos = lancamentosSaida.Retorno.Registros;
            saida.GerarPdf = gerarPdf;

            if (gerarPdf)
            {
                var footer = "--footer-right \"Emitido em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\" --footer-left \"Pagina: [page] de [toPage]\" --footer-line --footer-font-size \"7\" --footer-spacing 10 --footer-font-name \"Poppins\"";

                //return new ViewAsPdf("PopupExtratoPorPeriodo", saida)
                //{
                //    CustomSwitches = footer,
                //    PageOrientation = Orientation.Portrait,
                //    FileName = $"extrato_por_periodo_{dataInicio.Value.ToString("ddMMyyyy")}_{dataFim.Value.ToString("ddMMyyyy")}.pdf",
                //    PageMargins = new Margins(5, 3, 5, 3)
                //};

                return PartialView("PopupExtratoPorPeriodo", saida);
            }

            return PartialView("PopupExtratoPorPeriodo", saida);
        }
    }
}
