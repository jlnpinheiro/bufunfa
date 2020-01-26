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
    [Route("atalhos")]
    public class AtalhoController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public AtalhoController(DatatablesHelper datatablesHelper, BackendProxy proxy)
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

        [HttpGet]
        [Route("listar-atalhos")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de atalhos cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarAtalhos()
        {
            var saida = await _proxy.ObterAtalhos();

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a relação de atalhos cadastrados.", saida.Mensagens));

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno?.Count() ?? 0, saida.Retorno ?? new List<Atalho>());
        }

        [HttpGet]
        [Route("cadastrar-atalho")]
        public IActionResult CadastrarAtalho()
        {
            return PartialView("Manter", null);
        }

        [HttpPost]
        [Route("cadastrar-atalho")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo atalho.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarAtalho(ManterAtalho entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do atalho não foram preenchidas.", new[] { "Verifique se todas as informações do atalho foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarAtalho(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o atalho.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-atalho")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do atalho.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarAtalho(int id)
        {
            var saida = await _proxy.ObterAtalhoPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do atalho.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-atalho")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do atalho.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarAtalho(ManterAtalho entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do atalho não foram preenchidas.", new[] { "Verifique se todas as informações do atalho foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.AlterarAtalho(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o atalho.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-atalho")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o atalho.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirAtalho(int id)
        {
            var saida = await _proxy.ExcluirAtalho(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir o atalho.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Atalho excluído com sucesso."));
        }
    }
}
