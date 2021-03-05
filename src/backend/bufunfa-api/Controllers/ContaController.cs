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
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Notificações existentes. (Bad Request)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Endereço não encontrado. (Not found)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Acesso negado. Token de autenticação não encontrado. (Unauthorized)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.Unauthorized, typeof(UnauthorizedApiResponse))]
    [SwaggerTag("Permite a gestão e consulta das contas.")]
    public class ContaController : BaseController
    {
        private readonly ContaServico _contaServico;

        public ContaController(ContaServico contaServico)
        {
            this._contaServico = contaServico;
        }

        /// <summary>
        /// Obtém um conta a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("conta/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterContaPorIdResponseExemplo))]
        public async Task<IActionResult> ObterContaPorId([FromQuery, SwaggerParameter("ID da conta.", Required = true)] int idConta)
        {
            return new ApiResult(await _contaServico.ObterContaPorId(idConta, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém as contas do usuário autenticado
        /// </summary>
        [HttpGet]
        [Route("conta/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Contas do usuário encontradas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterContasPorUsuarioResponseExemplo))]
        public async Task<IActionResult> ObterContasPorUsuario()
        {
            return new ApiResult(await _contaServico.ObterContasPorUsuario(base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém a análise de um determinado ativo
        /// </summary>
        [HttpGet]
        [Route("conta/obter-analise-ativo")]
        [SwaggerOperation(Description = "Caso o valor da cotação não seja informado, o valor é obtido através da consulta à API da Alpha Vantage (https://www.alphavantage.co/documentation/).")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Análise do ativo obtida com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterAnaliseAcaoResponseExemplo))]
        public async Task<IActionResult> ObterAnaliseAcao(
            [FromQuery, SwaggerParameter("ID da ativo (conta).", Required = true)] int idAcao, 
            [FromQuery, SwaggerParameter("Valor da cotação do ativo.", Required = false)] decimal valorCotacao = 0)
        {
            return new ApiResult(await _contaServico.ObterAnaliseAtivo(idAcao, base.ObterIdUsuarioClaim(), valorCotacao));
        }

        /// <summary>
        /// Obtém a análise dos ativos do usuário autenticado
        /// </summary>
        [HttpGet]
        [Route("conta/obter-analise-ativos")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Análise dos ativos obtida com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterAnaliseAcoesResponseExemplo))]
        public async Task<IActionResult> ObterAnaliseAtivosPorUsuario()
        {
            return new ApiResult(await _contaServico.ObterAnaliseAtivosPorUsuario(base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza o cadastro de uma nova conta.
        /// </summary>
        [HttpPost]
        [Route("conta/cadastrar")]
        [SwaggerRequestExample(typeof(ContaViewModel), typeof(ContaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarContaResponseExemplo))]
        public async Task<IActionResult> CadastrarConta([FromBody, SwaggerParameter("Informações de cadastro da conta.", Required = true)] ContaViewModel model)
        {
            var entrada = new ContaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.Tipo,
                model.ValorSaldoInicial,
                model.NomeInstituicao,
                model.NumeroAgencia,
                model.Numero,
                model.Ranking);

            return new ApiResult(await _contaServico.CadastrarConta(entrada));
        }

        /// <summary>
        /// Realiza a transferência de valores entre contas
        /// </summary>
        [HttpPost]
        [Route("conta/realizar-transferencia")]
        [SwaggerRequestExample(typeof(TransferenciaViewModel), typeof(TransferenciaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Transferência realizada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(RealizarTransferenciaResponseExemplo))]
        public async Task<IActionResult> RealizarTransferencia([FromBody, SwaggerParameter("Informações da transferência.", Required = true)] TransferenciaViewModel model)
        {
            var entrada = new TransferenciaEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdContaOrigem.Value,
                model.IdContaDestino.Value,
                model.Data.Value,
                model.Valor.Value,
                model.Observacao);

            return new ApiResult(await _contaServico.RealizarTransferencia(entrada));
        }

        /// <summary>
        /// Realiza a alteração de uma conta.
        /// </summary>
        [HttpPut]
        [Route("conta/alterar")]
        [SwaggerRequestExample(typeof(ContaViewModel), typeof(ContaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarContaResponseExemplo))]
        public async Task<IActionResult> AlterarConta(
            [FromQuery, SwaggerParameter("ID da conta.", Required = true)] int idConta,
            [FromBody, SwaggerParameter("Informações para alteração da conta.", Required = true)] ContaViewModel model)
        {
            var entrada = new ContaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.Tipo,
                model.ValorSaldoInicial,
                model.NomeInstituicao,
                model.NumeroAgencia,
                model.Numero,
                model.Ranking);

            return new ApiResult(await _contaServico.AlterarConta(idConta, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de uma conta.
        /// </summary>
        [HttpDelete]
        [Route("conta/excluir")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Conta excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirContaResponseExemplo))]
        public async Task<IActionResult> ExcluirConta([FromQuery, SwaggerParameter("ID da conta que deverá ser excluída.", Required = true)] int idConta)
        {
            return new ApiResult(await _contaServico.ExcluirConta(idConta, base.ObterIdUsuarioClaim()));
        }
    }
}
