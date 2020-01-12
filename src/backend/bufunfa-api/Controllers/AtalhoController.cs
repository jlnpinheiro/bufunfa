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
    [SwaggerTag("Permite a gestão e consulta dos atalhos.")]
    public class AtalhoController : BaseController
    {
        private readonly IAtalhoServico _atalhoServico;

        public AtalhoController(IAtalhoServico atalhoServico)
        {
            _atalhoServico = atalhoServico;
        }

        /// <summary>
        /// Obtém um atalho a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("atalho/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Atalho encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterAtalhoPorIdResponseExemplo))]
        public async Task<IActionResult> ObterContaPorId([FromQuery, SwaggerParameter("ID do atalho.", Required = true)] int idAtalho)
        {
            return new ApiResult(await _atalhoServico.ObterAtalhoPorId(idAtalho, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém os atalhos do usuário autenticado
        /// </summary>
        [HttpGet]
        [Route("atalho/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Atalhos do usuário encontrados.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterAtalhosPorUsuarioResponseExemplo))]
        public async Task<IActionResult> ObterAtalhosPorUsuario()
        {
            return new ApiResult(await _atalhoServico.ObterAtalhosPorUsuario(base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza o cadastro de um novo atalho.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("atalho/cadastrar")]
        [SwaggerRequestExample(typeof(AtalhoViewModel), typeof(AtalhoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Atalho cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarAtalhoResponseExemplo))]
        public async Task<IActionResult> CadastrarAtalho([FromBody, SwaggerParameter("Informações de cadastro do atalho.", Required = true)] AtalhoViewModel viewModel)
        {
            var entrada = new AtalhoEntrada(
                base.ObterIdUsuarioClaim(),
                viewModel.Titulo,
                viewModel.Url);

            return new ApiResult(await _atalhoServico.CadastrarAtalho(entrada));
        }

        /// <summary>
        /// Realiza a alteração de um atalho.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("atalho/alterar")]
        [SwaggerRequestExample(typeof(AtalhoViewModel), typeof(AtalhoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Atalho alterado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarAtalhoResponseExemplo))]
        public async Task<IActionResult> AlterarAtalho(
            [FromQuery, SwaggerParameter("ID do atalho.", Required = true)] int idAtalho,
            [FromBody, SwaggerParameter("Informações para alteração de um atalho.", Required = true)] AtalhoViewModel model)
        {
            var entrada = new AtalhoEntrada(
                base.ObterIdUsuarioClaim(),
                model.Titulo,
                model.Url);

            return new ApiResult(await _atalhoServico.AlterarAtalho(idAtalho, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de um atalho.
        /// </summary>
        [HttpDelete]
        [Route("atalho/excluir")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Atalho excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirAtalhoResponseExemplo))]
        public async Task<IActionResult> ExcluirAtalho([FromQuery, SwaggerParameter("ID do atalho que deverá ser excluído.", Required = true)] int idAtalho)
        {
            return new ApiResult(await _atalhoServico.ExcluirAtalho(idAtalho, base.ObterIdUsuarioClaim()));
        }
    }
}
