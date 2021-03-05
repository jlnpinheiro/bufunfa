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
    [SwaggerTag("Permite a gestão e consulta das categorias atribuídas a lançamentos e agendamentos.")]
    public class CategoriaController : BaseController
    {
        private readonly CategoriaServico _categoriaServico;

        public CategoriaController(CategoriaServico categoriaServico)
        {
            _categoriaServico = categoriaServico;
        }

        /// <summary>
        /// Obtém uma categoria a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("categoria/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria encontrada.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCategoriaPorIdResponseExemplo))]
        public async Task<IActionResult> ObterCategoriaPorId([FromQuery, SwaggerParameter("ID da categoria.", Required = true)] int idCategoria)
        {
            return new ApiResult(await _categoriaServico.ObterCategoriaPorId(idCategoria, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém as categorias do usuário autenticado
        /// </summary>
        [HttpGet]
        [Route("categoria/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categorias do usuário encontradas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterCategoriasPorUsuarioResponseExemplo))]
        public async Task<IActionResult> ObterCategoriasPorUsuario()
        {
            return new ApiResult(await _categoriaServico.ObterCategoriasPorUsuario(base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza uma procura por categorias a partir dos parâmetros informados
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("categoria/procurar")]
        [SwaggerRequestExample(typeof(ProcurarCategoriaViewModel), typeof(ProcurarCategoriaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por categorias.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarCategoriaResponseExemplo))]
        public async Task<IActionResult> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarCategoriaViewModel model)
        {
            var entrada = new ProcurarCategoriaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.IdCategoriaPai,
                model.Tipo,
                model.Caminho);

            return new ApiResult(await _categoriaServico.ProcurarCategorias(entrada));
        }

        /// <summary>
        /// Realiza o cadastro de uma nova categoria.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("categoria/cadastrar")]
        [SwaggerRequestExample(typeof(CategoriaViewModel), typeof(CategoriaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarCategoriaResponseExemplo))]
        public async Task<IActionResult> CadastrarCategoria([FromBody, SwaggerParameter("Informações de cadastro da categoria.", Required = true)] CategoriaViewModel model)
        {
            var entrada = new CategoriaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.Tipo,
                model.IdCategoriaPai);

            return new ApiResult(await _categoriaServico.CadastrarCategoria(entrada));
        }

        /// <summary>
        /// Realiza a alteração de uma categoria.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("categoria/alterar")]
        [SwaggerRequestExample(typeof(CategoriaViewModel), typeof(CategoriaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarCategoriaResponseExemplo))]
        public async Task<IActionResult> AlterarCategoria(
            [FromQuery, SwaggerParameter("ID da categoria.", Required = true)] int idCategoria,
            [FromBody, SwaggerParameter("Informações para alteração de uma categoria.", Required = true)] CategoriaViewModel model)
        {
            var entrada = new CategoriaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.Tipo,
                model.IdCategoriaPai);

            return new ApiResult(await _categoriaServico.AlterarCategoria(idCategoria, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de uma categoria.
        /// </summary>
        [HttpDelete]
        [Route("categoria/excluir")]
        [SwaggerOperation(Description = "<b>Atenção - </b> Nas seguintes situações a exclusão da categoria não será permitida:<br><ul><li>Caso a categoria possua categorias-filha.</li><li>Caso a categoria possua lançamentos associados.</li></ul>")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Categoria excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirCategoriaResponseExemplo))]
        public async Task<IActionResult> ExcluirCategoria([FromQuery, SwaggerParameter("ID da categoria que deverá ser excluída.", Required = true)] int idCategoria)
        {
            return new ApiResult(await _categoriaServico.ExcluirCategoria(idCategoria, base.ObterIdUsuarioClaim()));
        }
    }
}
