using JNogueira.Bufunfa.Api.Binders;
using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;
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
    [SwaggerTag("Permite a gestão e consulta dos períodos.")]
    public class PeriodoController : BaseController
    {
        private readonly IPeriodoServico _periodoServico;

        public PeriodoController(IPeriodoServico periodoServico)
        {
            _periodoServico = periodoServico;
        }

        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("periodo/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodoPorIdResponseExemplo))]
        public async Task<IActionResult> ObterPeriodoPorId([FromQuery, SwaggerParameter("ID do período.", Required = true)] int idPeriodo)
        {
            return new ApiResult(await _periodoServico.ObterPeriodoPorId(idPeriodo, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém um período que compreenda a data informada
        /// </summary>
        [HttpGet]
        [Route("periodo/obter-por-data")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodoPorIdResponseExemplo))]
        public async Task<IActionResult> ObterPeriodoPorData([FromQuery, DateTimeModelBinder(DateFormat = "dd-MM-yyyy"), SwaggerParameter("Data que deverá estar compreendida pelo período (formato dd-mm-aaaa).", Required = true)] DateTime? data)
        {
            return new ApiResult(await _periodoServico.ObterPeriodoPorData(data.Value, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém os períodos do usuário autenticado
        /// </summary>
        [HttpGet]
        [Route("periodo/obter")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Períodos do usuário encontrados.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterPeriodosPorUsuarioResponseExemplo))]
        public async Task<IActionResult> ObterPeriodosPorUsuarioAutenticado()
        {
            return new ApiResult(await _periodoServico.ObterPeriodosPorUsuario(base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza uma procura por períodos a partir dos parâmetros informados
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("periodo/procurar")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por períodos.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarPeriodoResponseExemplo))]
        public async Task<IActionResult> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarPeriodoViewModel model)
        {
            var entrada = new ProcurarPeriodoEntrada(
                base.ObterIdUsuarioClaim(),
                model.OrdenarPor,
                model.OrdenarSentido,
                model.PaginaIndex,
                model.PaginaTamanho)
            {
                Nome = model.Nome,
                Data = model.Data
            };

            return new ApiResult(await _periodoServico.ProcurarPeriodos(entrada));
        }

        /// <summary>
        /// Realiza o cadastro de um novo período.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("periodo/cadastrar")]
        [SwaggerRequestExample(typeof(PeriodoViewModel), typeof(CadastrarPeriodoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarPeriodoResponseExemplo))]
        public async Task<IActionResult> CadastrarPeriodo([FromBody, SwaggerParameter("Informações de cadastro do período.", Required = true)] PeriodoViewModel model)
        {
            var entrada = new PeriodoEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.DataInicio.Value,
                model.DataFim.Value);

            return new ApiResult(await _periodoServico.CadastrarPeriodo(entrada));
        }

        /// <summary>
        /// Realiza a alteração de um período.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("periodo/alterar")]
        [SwaggerRequestExample(typeof(PeriodoViewModel), typeof(PeriodoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período alterado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarPeriodoResponseExemplo))]
        public async Task<IActionResult> AlterarPeriodo(
            [FromQuery, SwaggerParameter("ID do período.", Required = true)] int idPeriodo,
            [FromBody, SwaggerParameter("Informações para alteração de um período.", Required = true)] PeriodoViewModel model)
        {
            var entrada = new PeriodoEntrada(
                base.ObterIdUsuarioClaim(),
                model.Nome,
                model.DataInicio.Value,
                model.DataFim.Value);

            return new ApiResult(await _periodoServico.AlterarPeriodo(idPeriodo, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de um período.
        /// </summary>
        [HttpDelete]
        [Route("periodo/excluir")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Período excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirPeridoResponseExemplo))]
        public async Task<IActionResult> ExcluirPeriodo([FromQuery, SwaggerParameter("ID do período que deverá ser excluído.", Required = true)] int idPeriodo)
        {
            return new ApiResult(await _periodoServico.ExcluirPeriodo(idPeriodo, base.ObterIdUsuarioClaim()));
        }
    }
}
