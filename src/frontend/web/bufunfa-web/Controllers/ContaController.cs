using JNogueira.Bufunfa.Web.Filters;
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
    [Route("contas")]
    public class ContaController : BaseController
    {
        public ContaController(BackendProxy proxy)
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
        [Route("listar-contas")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as contas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> Listar(string tipo)
        {
            if (tipo == "RF")
            {
                var saida = await _proxy.ObterContas();

                if (!saida.Sucesso)
                    return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as contas.", saida.Mensagens));

                return PartialView("ListarRendaFixa", saida.Retorno?.Where(x => x.TipoInvestimento == null || x.TipoInvestimento == TipoInvestimento.RendaFixa));
            }
           
            var analiseSaida = await _proxy.ObterAnaliseAtivos();

            if (!analiseSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter os ativos.", analiseSaida.Mensagens));

            return PartialView("ListarRendaVariavel", analiseSaida.Retorno);
        }

        [HttpGet]
        [Route("cadastrar-conta")]
        public IActionResult CadastrarConta(string tipo)
        {
            return PartialView(tipo == "RF" ? "ManterRendaFixa" : "ManterRendaVariavel", null);
        }

        [HttpPost]
        [Route("cadastrar-conta")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar a nova conta.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarConta(ManterConta entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da conta não foram preenchidas.", new[] { "Verifique se todas as informações da conta foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarConta(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a conta.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-conta")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da conta.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarConta(int idConta)
        {
            var saida = await _proxy.ObterContaPorId(idConta);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da conta.", saida.Mensagens));

            return PartialView(saida.Retorno.CodigoTipo != (int)TipoConta.Acoes && saida.Retorno.CodigoTipo != (int)TipoConta.FII ? "ManterRendaFixa" : "ManterRendaVariavel", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-conta")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações da conta.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarConta(ManterConta entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da conta não foram preenchidas.", new[] { "Verifique se todas as informações da conta foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.AlterarConta(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a conta.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-conta")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir a conta.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirConta(int id)
        {
            var saida = await _proxy.ExcluirConta(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir a conta.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Conta excluída com sucesso."));
        }

        [HttpGet]
        [Route("obter-analise-por-ativo")]
        public async Task<IActionResult> ObterAnalisePorAtivo(int id, decimal valorCotacao = 0)
        {
            var saida = await _proxy.ObterAnaliseAtivo(id, valorCotacao);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a análise do ativo.", saida.Mensagens));

            return PartialView("PopupAcao", saida.Retorno);
        }

        [HttpGet]
        [Route("informar-valor-cotacao-por-acao")]
        public async Task<IActionResult> InformarValorCotacaoPorAcao(int id)
        {
            var saida = await _proxy.ObterContaPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações da ação.", saida.Mensagens));

            return PartialView("PopupValorCotacao", saida.Retorno);
        }

        

        [HttpGet]
        [Route("obter-saldo-atual")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter o saldo atual da conta.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ObterContaSaldoAtual(int id)
        {
            var saida = await _proxy.ObterContaPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações da conta.", saida.Mensagens));

            return PartialView("ContaSaldoAtual", saida.Retorno);
        }

        [HttpGet]
        [Route("popup-contas-lancamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as contas para exibição do popup.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ObterPopupContasLancamento()
        {
            var saida = await _proxy.ObterContas();

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações das contas.", saida.Mensagens));

            return PartialView("PopupConta", saida.Retorno?.Where(x => x.CodigoTipo != (int)TipoConta.Acoes).ToList());
        }

        [HttpGet]
        [Route("realizar-transferencia")]
        public IActionResult RealizarTransferencia()
        {
            return PartialView("Transferir");
        }

        [HttpPost]
        [Route("realizar-transferencia")]
        [FeedbackExceptionFilter("Ocorreu um erro ao realizar a transferência.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> RealizarTransferencia(Transferir entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da transferência não foram preenchidas.", new[] { "Verifique se todas as informações da transferência foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.RealizarTransferencia(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível realizar a transferência.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }
    }
}

