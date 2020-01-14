using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("pessoas")]
    public class PessoaController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public PessoaController(DatatablesHelper datatablesHelper, BackendProxy proxy)
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
        [Route("listar-pessoas")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de pessoas cadastradas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarPessoas(ProcurarPessoa filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            filtro.Nome           = _datatablesHelper.PalavraChave;
            filtro.OrdenarPor     = _datatablesHelper.OrdenarPor;
            filtro.OrdenarSentido = _datatablesHelper.OrdenarSentido;
            filtro.PaginaIndex    = _datatablesHelper.PaginaIndex;
            filtro.PaginaTamanho  = _datatablesHelper.PaginaTamanho;

            var saida = await _proxy.ProcurarPessoas(filtro);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a relação de pessoas cadastradas.", saida.Mensagens));

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.Retorno.Registros);
        }

        [HttpGet]
        [Route("cadastrar-pessoa")]
        public IActionResult CadastrarPessoa()
        {
            return PartialView("Manter", null);
        }

        [HttpPost]
        [Route("cadastrar-pessoa")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar a nova pessoa.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarPessoa(ManterPessoa entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da pessoa não foram preenchidas.", new[] { "Verifique se todas as informações da pessoa foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarPessoa(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a pessoa.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-pessoa")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da pessoa.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarPessoa(int id)
        {
            var saida = await _proxy.ObterPessoaPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da pessoa.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-pessoa")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações da pessoa.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarPessoa(ManterPessoa entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da pessoa não foram preenchidas.", new[] { "Verifique se todas as informações da pessoa foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.AlterarPessoa(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a pessoa.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-pessoa")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir a pessoa.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirPessoa(int id)
        {
            var saida = await _proxy.ExcluirPessoa(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a pessoa.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Pessoa excluída com sucesso."));
        }

        [HttpGet]
        [Route("obter-pessoas-por-palavra-chave")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de pessoas cadastradas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ObterPessoasPorPalavraChave(string palavraChave)
        {
            var filtro = new ProcurarPessoa
            {
                Nome = palavraChave,
                OrdenarPor = "Nome",
                OrdenarSentido = "ASC",
                PaginaIndex = 1,
                PaginaTamanho = 10
            };

            var saida = await _proxy.ProcurarPessoas(filtro);

            return new JsonResult(saida.Retorno.Registros);
        }
    }
}
