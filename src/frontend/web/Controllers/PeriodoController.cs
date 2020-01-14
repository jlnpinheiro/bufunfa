using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
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
    [Route("periodos")]
    public class PeriodoController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public PeriodoController(DatatablesHelper datatablesHelper, BackendProxy proxy)
            : base(proxy)
        {
            _datatablesHelper = datatablesHelper;
        }

        [HttpGet]
        [ExibirPeriodoAtualFilter]
        [ExibirAtalhosFilter]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("listar-periodos")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de períodos cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarPeriodos(ProcurarPeriodo filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            filtro.Nome           = _datatablesHelper.PalavraChave;
            filtro.OrdenarPor     = _datatablesHelper.OrdenarPor;
            filtro.OrdenarSentido = _datatablesHelper.OrdenarSentido;
            filtro.PaginaIndex    = _datatablesHelper.PaginaIndex;
            filtro.PaginaTamanho  = _datatablesHelper.PaginaTamanho;

            var saida = await _proxy.ProcurarPeriodos(filtro);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a relação de períodos cadastrados.", saida.Mensagens));

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.Retorno.Registros?.Select(x => new {
                id = x.Id,
                nome = x.Nome,
                dataInicio = x.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"),
                dataFim = x.DataFim.ToString("dd/MM/yyyy HH:mm:ss"),
                quantidadeDias = x.QuantidadeDias
            }));
        }

        [HttpGet]
        [Route("cadastrar-periodo")]
        public IActionResult CadastrarPeriodo()
        {
            return PartialView("Manter", null);
        }

        [HttpPost]
        [Route("cadastrar-periodo")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo período.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarPeriodo(ManterPeriodo entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do período não foram preenchidas.", new[] { "Verifique se todas as informações do período foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarPeriodo(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o período.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-periodo")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do período.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarPeriodo(int id)
        {
            var saida = await _proxy.ObterPeriodoPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do período.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-periodo")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do período.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarPeriodo(ManterPeriodo entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do período não foram preenchidas.", new[] { "Verifique se todas as informações do período foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.AlterarPeriodo(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o período.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-periodo")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o período.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirPeriodo(int id)
        {
            var saida = await _proxy.ExcluirPeriodo(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o período.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Período excluído com sucesso."));
        }

        [HttpGet]
        [Route("obter-periodos-por-palavra-chave")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de períodos cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ObterPeriodosPorPalavraChave(string palavraChave)
        {
            var filtro = new ProcurarPeriodo
            {
                Nome = palavraChave,
                OrdenarPor = "DataInicio",
                OrdenarSentido = "DESC",
                PaginaIndex = 1,
                PaginaTamanho = 10
            };

            var saida = await _proxy.ProcurarPeriodos(filtro);

            return new JsonResult(saida.Retorno.Registros?.Select(x => new
            {
                id = x.Id,
                nome = x.Nome,
                dataInicio = x.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"),
                dataFim = x.DataFim.ToString("dd/MM/yyyy HH:mm:ss"),
                quantidadeDias = x.QuantidadeDias
            }).ToArray());
        }

        [HttpGet]
        [Route("detalhar-periodo-atual")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter o período atual.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> DetalharPeriodoAtual()
        {
            var saida = await _proxy.ObterPeriodoPorDataReferencia(DateTime.Now);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível visulizar o detalhamento do período atual.", saida.Mensagens));

            return PartialView("DetalharPeriodoAtual", saida.Retorno);
        }
    }
}
