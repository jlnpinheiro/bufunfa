using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.Net;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Api.Controllers
{
    [Authorize("Bearer")]
    [Produces("application/json")]
    [SwaggerResponse((int)HttpStatusCode.InternalServerError, "Erro não tratado encontrado. (Internal Server Error)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.InternalServerError, typeof(InternalServerErrorApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Endereço não encontrado. (Not found)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Acesso negado. Token de autenticação não encontrado. (Unauthorized)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.Unauthorized, typeof(UnauthorizedApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Forbidden, "Acesso negado. Sem permissão de acesso a funcionalidade. (Forbidden)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.Forbidden, typeof(ForbiddenApiResponse))]
    [SwaggerTag("Permite a gestão e consulta dos cartões de crédito.")]
    public class CartaoCreditoController : BaseController
    {
        private readonly CartaoCreditoServico _cartaoCreditoServico;

        public CartaoCreditoController(CartaoCreditoServico cartaoCreditoServico)
        {
            this._cartaoCreditoServico = cartaoCreditoServico;
        }

        /// <summary>
        /// Obtém um cartão de crédito a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("cartao-credito/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCartaoCreditoPorIdResponseExemplo))]
        public async Task<IActionResult> ObterCartaoCreditoPorId([FromQuery, SwaggerParameter("ID do cartão de crédito.", Required = true)] int idCartaoCredito)
        {
            return new ApiResult(await _cartaoCreditoServico.ObterCartaoCreditoPorId(idCartaoCredito, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém os cartões do usuário autenticado
        /// </summary>
        [HttpGet]
        [Route("cartao-credito/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartões do usuário encontradas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCartaoCreditosPorUsuarioResponseExemplo))]
        public async Task<IActionResult> ObterCartaoCreditosPorUsuarioAutenticado()
        {
            return new ApiResult(await _cartaoCreditoServico.ObterCartoesCreditoPorUsuario(base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém uma fatura do cartão de crédito, a partir do mês e ano da fatura
        /// </summary>
        [HttpGet]
        [Route("cartao-credito/obter-fatura")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Fatura do cartão encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterFaturaPorCartaoCreditoResponseExemplo))]
        public async Task<IActionResult> ObterFaturaPorCartaoCredito(
            [FromQuery, SwaggerParameter("ID do cartão de crédito.", Required = true)] int idCartaoCredito,
            [FromQuery, SwaggerParameter("Mês da fatura do cartão de crédito.", Required = true)] int mesFatura,
            [FromQuery, SwaggerParameter("Ano da fatura do cartão de crédito.", Required = true)] int anoFatura)
        {
            return new ApiResult(await _cartaoCreditoServico.ObterFaturaPorCartaoCredito(idCartaoCredito, base.ObterIdUsuarioClaim(), mesFatura, anoFatura));
        }

        /// <summary>
        /// Obtém uma fatura do cartão de crédito, a partir do lançamento associado ao pagamento dessa fatura
        /// </summary>
        [HttpGet]
        [Route("cartao-credito/obter-fatura-por-lancamento")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Fatura do cartão encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterFaturaPorCartaoCreditoResponseExemplo))]
        public async Task<IActionResult> ObterFaturaPorLancamento(
            [FromQuery, SwaggerParameter("ID do lançamento associado ao pagamento da fatura.", Required = true)] int idLancamento)
        {
            return new ApiResult(await _cartaoCreditoServico.ObterFaturaPorLancamento(idLancamento, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza o cadastro de um novo cartão.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("cartao-credito/cadastrar")]
        [SwaggerRequestExample(typeof(CartaoCreditoViewModel), typeof(CartaoCreditoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarCartaoCreditoResponseExemplo))]
        public async Task<IActionResult> CadastrarCartaoCredito([FromBody, SwaggerParameter("Informações de cadastro do cartão.", Required = true)] CartaoCreditoViewModel model)
        {
            var entrada = new CartaoCreditoEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.ValorLimite.Value,
                model.DiaVencimentoFatura.Value);

            return new ApiResult(await _cartaoCreditoServico.CadastrarCartaoCredito(entrada));
        }

        /// <summary>
        /// Realiza o pagamento de uma fatura de cartão de crédito
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("cartao-credito/pagar-fatura")]
        [SwaggerRequestExample(typeof(PagarFaturaViewModel), typeof(PagarFaturaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pagamento da fatura realizado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(PagarFaturaResponseExemplo))]
        public async Task<IActionResult> PagarFatura([FromBody, SwaggerParameter("Informações referentes ao pagamento da fatura.", Required = true)] PagarFaturaViewModel model)
        {
            var entrada = new PagarFaturaEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdCartaoCredito.Value,
                model.MesFatura.Value,
                model.AnoFatura.Value,
                model.IdContaPagamento.Value,
                model.DataPagamento.Value,
                model.ValorPagamento.Value,
                model.IdPessoaPagamento,
                model.ValorAdicionalCredito,
                model.ObservacaoCredito,
                model.ValorAdicionalDebito,
                model.ObservacaoDebito);

            return new ApiResult(await _cartaoCreditoServico.PagarFatura(entrada));
        }

        /// <summary>
        /// Realiza a alteração de um cartão.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("cartao-credito/alterar")]
        [SwaggerRequestExample(typeof(CartaoCreditoViewModel), typeof(CartaoCreditoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão alterado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarCartaoCreditoResponseExemplo))]
        public async Task<IActionResult> AlterarCartaoCredito(
            [FromQuery, SwaggerParameter("ID do cartão de crédito.", Required = true)] int idCartaoCredito,
            [FromBody, SwaggerParameter("Informações para alteração do cartão.", Required = true)] CartaoCreditoViewModel model)
        {
            var entrada = new CartaoCreditoEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,               
                model.ValorLimite.Value,
                model.DiaVencimentoFatura.Value);

            return new ApiResult(await _cartaoCreditoServico.AlterarCartaoCredito(idCartaoCredito, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de um cartão.
        /// </summary>
        [HttpDelete]
        [Route("cartao-credito/excluir")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Cartão excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirCartaoCreditoResponseExemplo))]
        public async Task<IActionResult> ExcluirCartaoCredito([FromQuery, SwaggerParameter("ID do cartão que deverá ser excluído.", Required = true)] int idCartaoCredito)
        {
            return new ApiResult(await _cartaoCreditoServico.ExcluirCartaoCredito(idCartaoCredito, base.ObterIdUsuarioClaim()));
        }
    }
}
