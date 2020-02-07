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

        [HttpPost]
        [Route("gerar-extrato-por-periodo")]
        public async Task<IActionResult> GerarExtratoPorPeriodo(RelatorioExtratoPorPeriodoEntrada entrada)
        {
            var contaSaida = await _proxy.ObterContaPorId(entrada.IdConta);

            if (!contaSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da conta.", contaSaida.Mensagens));

            Periodo periodo = null;

            if (entrada.IdPeriodo.HasValue)
            {
                var periodoSaida = await _proxy.ObterPeriodoPorId(entrada.IdPeriodo.Value);

                if (!periodoSaida.Sucesso)
                    return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações do período.", periodoSaida.Mensagens));

                periodo = periodoSaida.Retorno;

                entrada.DataInicio = periodo.DataInicio;
                entrada.DataFim = periodo.DataFim;
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
                saida.DataInicio = entrada.DataInicio;
                saida.DataFim    = entrada.DataFim;
            }

            var lancamentosSaida = await _proxy.ProcurarLancamentos(new ProcurarLancamento { IdConta = entrada.IdConta, DataInicio = entrada.DataInicio, DataFim = entrada.DataFim });

            if (!lancamentosSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter os lançamento do período informado para a conta.", lancamentosSaida.Mensagens));

            saida.Lancamentos = lancamentosSaida.Retorno.Registros;

            if (entrada.GerarPdf)
            {
                var footer = "--footer-right \"Emitido em: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "\" --footer-left \"Página: [page] de [toPage]\" --footer-line --footer-font-size \"7\" --footer-spacing 1 --footer-font-name \"Poppins\"";

                return new ViewAsPdf("RelatorioTeste", saida)
                {
                    CustomSwitches = footer,
                    PageOrientation = Orientation.Portrait,
                    FileName = "relatorio_teste.pdf",
                    PageMargins = new Margins(5, 3, 5, 3)
                };
            }

            return PartialView("RelatorioTeste", saida);
        }
    }
}
