using JNogueira.Bufunfa.Web.Filters;
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
    [Route("cartoes")]
    public class CartaoCreditoController : BaseController
    {
        public CartaoCreditoController(BackendProxy proxy)
            : base(proxy)
        {
        }

        [HttpGet]
        [Route("listar-cartoes")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter os cartões de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> Listar()
        {
            var saida = await _proxy.ObterCartoesCredito();

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter os cartões de crédito.", saida.Mensagens));

            return PartialView("Listar", saida.Retorno);
        }

        [HttpGet]
        [Route("cadastrar-cartao")]
        public IActionResult CadastrarCartao()
        {
            return PartialView("Manter", null);
        }

        [HttpPost]
        [Route("cadastrar-cartao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo cartão de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarCartao(ManterCartaoCredito entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do cartão de crédito não foram preenchidas.", new[] { "Verifique se todas as informações do cartão de crédito foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarCartaoCredito(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o cartão de crédito.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-cartao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do cartão de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCartao(int id)
        {
            var saida = await _proxy.ObterCartaoCreditoPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do cartão de crédito.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-cartao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do cartão de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCartao(ManterCartaoCredito entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do cartão de crédito não foram preenchidas.", new[] { "Verifique se todas as informações do cartão de crédito foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.AlterarCartaoCredito(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o cartão de crédito.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-cartao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o cartão de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirCartao(int id)
        {
            var saida = await _proxy.ExcluirCartaoCredito(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o cartão de crédito.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Cartão de crédito excluído com sucesso."));
        }

        [HttpGet]
        [Route("consultar-fatura")]
        public IActionResult ConsultarFatura()
        {
            return PartialView("ConsultarFatura");
        }

        [HttpGet]
        [Route("exibir-fatura")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a fatura do cartão de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExibirFatura(int idCartao, int mes, int ano)
        {
            var saida = await _proxy.ObterFaturaPorCartaoCredito(idCartao, mes, ano);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a fatura do cartão de crédito para o mês/ano informados.", saida.Mensagens));

            if (!saida.Retorno.Parcelas.Any())
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, $"Não existem parcelas abertas para a fatura de {new DateTime(ano, mes, 1).ToString("MMM/yyyy").ToUpper()} do cartão de crédito selecionado."));

            return PartialView("ExibirFatura", saida.Retorno);
        }

        [HttpGet]
        [Route("exibir-fatura-por-lancamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a fatura do cartão de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExibirFaturaPorLancamento(int idLancamento)
        {
            var saida = await _proxy.ObterFaturaPorLancamento(idLancamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a fatura do cartão de crédito a partir do lançamento.", saida.Mensagens));

            return PartialView("ExibirFatura", saida.Retorno);
        }

        [HttpGet]
        [Route("pagar-fatura")]
        public IActionResult PagarFatura(decimal valorFatura)
        {
            ViewBag.ValorFatura = valorFatura;
            
            return PartialView("PagarFatura");
        }

        [HttpPost]
        [Route("pagar-fatura")]
        [FeedbackExceptionFilter("Ocorreu um erro ao pagar a fatura do cartão de crédito.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> PagarFatura(PagarFatura entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações referentes ao pagamento da fatrura não foram preenchidas.", new[] { "Verifique se todas as informações referentes ao pagamento da fatura foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.PagarFatura(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível pagar a fatura do cartão de crédito.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }
    }
}
