using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
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
    [SwaggerTag("Permite a gestão e consulta das pessoas.")]
    public class PessoaController : BaseController
    {
        private readonly IPessoaServico _pessoaServico;

        public PessoaController(IPessoaServico pessoaServico)
        {
            _pessoaServico = pessoaServico;
        }

        /// <summary>
        /// Obtém uma pessoa a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("pessoa/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPessoaPorIdResponseExemplo))]
        public async Task<IActionResult> ObterContaPorId([FromQuery, SwaggerParameter("ID da pessoa.", Required = true)] int idPessoa)
        {
            return new ApiResult(await _pessoaServico.ObterPessoaPorId(idPessoa, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza uma procura por pessoas a partir dos parâmetros informados
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("pessoa/procurar")]
        [SwaggerRequestExample(typeof(ProcurarPessoaViewModel), typeof(ProcurarPessoaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por pessoas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarPessoaResponseExemplo))]
        public async Task<IActionResult> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarPessoaViewModel model)
        {
            var entrada = new ProcurarPessoaEntrada(
                base.ObterIdUsuarioClaim(),
                model.OrdenarPor,
                model.OrdenarSentido,
                model.PaginaIndex,
                model.PaginaTamanho)
            {
                Nome = model.Nome
            };

            return new ApiResult(await _pessoaServico.ProcurarPessoas(entrada));
        }

        /// <summary>
        /// Realiza o cadastro de uma nova pessoa.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("pessoa/cadastrar")]
        [SwaggerRequestExample(typeof(PessoaViewModel), typeof(PessoaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarPessoaResponseExemplo))]
        public async Task<IActionResult> CadastrarPessoa([FromBody, SwaggerParameter("Informações de cadastro da pessoa.", Required = true)] PessoaViewModel viewModel)
        {
            var entrada = new PessoaEntrada(
                base.ObterIdUsuarioClaim(),
                viewModel.Nome);

            return new ApiResult(await _pessoaServico.CadastrarPessoa(entrada));
        }

        /// <summary>
        /// Realiza a alteração de uma pessoa.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("pessoa/alterar")]
        [SwaggerRequestExample(typeof(PessoaViewModel), typeof(PessoaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarPessoaResponseExemplo))]
        public async Task<IActionResult> AlterarPessoa(
            [FromQuery, SwaggerParameter("ID da pessoa.", Required = true)] int idPessoa,
            [FromBody, SwaggerParameter("Informações para alteração de uma pessoa.", Required = true)] PessoaViewModel model)
        {
            var entrada = new PessoaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome);

            return new ApiResult(await _pessoaServico.AlterarPessoa(idPessoa, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de uma pessoa.
        /// </summary>
        [HttpDelete]
        [Route("pessoa/excluir")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Pessoa excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirPessoaResponseExemplo))]
        public async Task<IActionResult> ExcluirPessoa([FromQuery, SwaggerParameter("ID da pessoa que deverá ser excluída.", Required = true)] int idPessoa)
        {
            return new ApiResult(await _pessoaServico.ExcluirPessoa(idPessoa, base.ObterIdUsuarioClaim()));
        }
    }
}
