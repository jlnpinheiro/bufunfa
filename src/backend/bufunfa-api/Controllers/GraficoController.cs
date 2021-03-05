using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
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
    [SwaggerTag("Permite obter as informações utilizadas para geração de gráficos.")]
    public class GraficoController : BaseController
    {
        private readonly GraficoServico _graficoServico;

        public GraficoController(GraficoServico graficoServico)
        {
            _graficoServico = graficoServico;
        }

        /// <summary>
        /// Obtém as informações para geração do gráfico de relação entre valor e categoria por ano
        /// </summary>
        [HttpGet]
        [Route("grafico/obter-grafico-relacao-valor-categoria-por-ano")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Informações para geração do gráfico obtidas com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(GraficoRelacaoValorCategoriaPorAnoResponseExemplo))]
        public async Task<IActionResult> GerarGraficoRelacaoValorCategoriaPorAno(
            [FromQuery, SwaggerParameter("Ano que deverá ser analisado", Required = true)] int ano,
            [FromQuery, SwaggerParameter("ID da categoria.", Required = true)] int idCategoria)
        {
            return new ApiResult(await _graficoServico.GerarGraficoRelacaoValorCategoriaPorAno(ano, idCategoria, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém as informações específicas de um período que compõem o gráfico de relação entre valor e categoria por ano
        /// </summary>
        [HttpGet]
        [Route("grafico/obter-periodo-grafico-relacao-valor-categoria-por-ano")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Informações específicas do período obtidas com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodoRelacaoValorCategoriaPorAnoResponseExemplo))]
        public async Task<IActionResult> ObterPeriodoGraficoRelacaoValorCategoriaPorAno(
            [FromQuery, SwaggerParameter("ID do período", Required = true)] int idPeriodo,
            [FromQuery, SwaggerParameter("Ano que deverá ser analisado", Required = true)] int ano,
            [FromQuery, SwaggerParameter("ID da categoria.", Required = true)] int idCategoria)
        {
            return new ApiResult(await _graficoServico.ObterPeriodoGraficoRelacaoValorCategoriaPorAno(idPeriodo, ano, idCategoria, base.ObterIdUsuarioClaim()));
        }
    }
}
