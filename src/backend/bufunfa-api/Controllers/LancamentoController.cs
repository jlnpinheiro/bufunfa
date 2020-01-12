using JNogueira.Bufunfa.Api.Swagger;
using JNogueira.Bufunfa.Api.Swagger.Exemplos;
using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Interfaces.Servicos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System.IO;
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
    [SwaggerTag("Permite a gestão e consulta dos lançamentos.")]
    public class LancamentoController : BaseController
    {
        private readonly ILancamentoServico _lancamentoServico;
        private readonly ILancamentoAnexoRepositorio _anexoRepositorio;

        public LancamentoController(ILancamentoServico lancamentoServico, ILancamentoAnexoRepositorio anexoRepositorio)
        {
            _lancamentoServico = lancamentoServico;
            _anexoRepositorio = anexoRepositorio;
        }

        /// <summary>
        /// Obtém um lançamento a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("lancamento/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterLancamentoPorIdResponseExemplo))]
        public async Task<IActionResult> ObterLancamentoPorId([FromQuery, SwaggerParameter("ID do lançamento.", Required = true)] int idLancamento)
        {
            return new ApiResult(await _lancamentoServico.ObterLancamentoPorId(idLancamento, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza o download do arquivo de um anexo do lançamento, a partir do ID do anexo
        /// </summary>
        [HttpGet]
        [Produces(System.Net.Mime.MediaTypeNames.Application.Octet)]
        [Route("lancamento/realizar-download-anexo-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Download do arquivo do anexo realizado com sucesso.")]
        public async Task<IActionResult> RealizarDownloadLancamentoAnexoPorId([FromQuery, SwaggerParameter("ID do anexo.", Required = true)] int idAnexo)
        {
            var saida = await _lancamentoServico.ObterAnexoPorId(idAnexo, base.ObterIdUsuarioClaim());

            if (!saida.Sucesso)
                return new ApiResult(saida);

            var anexo = (LancamentoAnexoSaida)saida.Retorno;

            var stream = await _anexoRepositorio.Download(anexo.IdGoogleDrive);

            return File(stream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, anexo.NomeArquivo);
        }

        /// <summary>
        /// Realiza uma procura por lançamentos a partir dos parâmetros informados
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("lancamento/procurar")]
        [SwaggerRequestExample(typeof(ProcurarLancamentoViewModel), typeof(ProcurarLancamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por lançamentos.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarLancamentoResponseExemplo))]
        public async Task<IActionResult> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarLancamentoViewModel model)
        {
            var entrada = new ProcurarLancamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.OrdenarPor,
                model.OrdenarSentido,
                model.PaginaIndex,
                model.PaginaTamanho)
            {
                DataFim     = model.DataFim,
                DataInicio  = model.DataInicio,
                IdCategoria = model.IdCategoria,
                IdConta     = model.IdConta,
                IdPessoa    = model.IdPessoa
            };

            return new ApiResult(await _lancamentoServico.ProcurarLancamentos(entrada));
        }

        /// <summary>
        /// Realiza o cadastro de um novo lançamento.
        /// </summary>
        [Consumes("application/json")]
        [HttpPost]
        [Route("lancamento/cadastrar")]
        [SwaggerRequestExample(typeof(LancamentoViewModel), typeof(LancamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarLancamentoResponseExemplo))]
        public async Task<IActionResult> CadastrarLancamento([FromBody, SwaggerParameter("Informações de cadastro do lançamento.", Required = true)] LancamentoViewModel model)
        {
            var entrada = new LancamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdConta.Value,
                model.IdCategoria.Value,
                model.Data.Value,
                model.Valor.Value,
                model.QuantidadeAcoes,
                model.IdPessoa,
                null,
                model.Observacao);

            return new ApiResult(await _lancamentoServico.CadastrarLancamento(entrada));
        }

        /// <summary>
        /// Realiza o cadastro de um novo anexo para um lançamento.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [Route("lancamento/cadastrar-anexo")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Anexo cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarAnexoResponseExemplo))]
        public async Task<IActionResult> CadastrarAnexo(
            [FromQuery, SwaggerParameter("ID do lançamento.", Required = true)] int idLancamento,
            [FromForm, SwaggerParameter("Informações de cadastro do anexo.", Required = true)] LancamentoAnexoViewModel model)
        {
            LancamentoAnexoEntrada entrada;

            using (var memoryStream = new MemoryStream())
            {
                await model.Arquivo.CopyToAsync(memoryStream);

                entrada = new LancamentoAnexoEntrada(
                    base.ObterIdUsuarioClaim(),
                    model.Descricao,
                    model.NomeArquivo + model.Arquivo.FileName.Substring(model.Arquivo.FileName.LastIndexOf(".")),
                    memoryStream.ToArray(),
                    model.Arquivo.ContentType);
            }

            return new ApiResult(await _lancamentoServico.CadastrarAnexo(idLancamento, entrada));
        }

        /// <summary>
        /// Realiza o cadastro de um novo detalhe para um lançamento.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("lancamento/cadastrar-detalhe")]
        [SwaggerRequestExample(typeof(LancamentoDetalheViewModel), typeof(LancamentoDetalheRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Detalhe cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarDetalheResponseExemplo))]
        public async Task<IActionResult> CadastrarDetalhe(
            [FromQuery, SwaggerParameter("ID do lançamento.", Required = true)] int idLancamento,
            [FromBody, SwaggerParameter("Informações de cadastro do detalhe do lançamento.", Required = true)] LancamentoDetalheViewModel model)
        {
            var entrada = new LancamentoDetalheEntrada(model.IdCategoria.Value, model.Valor.Value, model.Observacao);

            return new ApiResult(await _lancamentoServico.CadastrarDetalhe(idLancamento, entrada));
        }

        /// <summary>
        /// Realiza a alteração de um lançamento.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("lancamento/alterar")]
        [SwaggerRequestExample(typeof(LancamentoViewModel), typeof(LancamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarLancamentoResponseExemplo))]
        public async Task<IActionResult> AlterarLancamento(
            [FromQuery, SwaggerParameter("ID do lançamento.", Required = true)] int idLancamento,
            [FromBody, SwaggerParameter("Informações para alteração de um lançamento.", Required = true)] LancamentoViewModel model)
        {
            var entrada = new LancamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdConta.Value,
                model.IdCategoria.Value,
                model.Data.Value,
                model.Valor.Value,
                model.QuantidadeAcoes,
                model.IdPessoa,
                null,
                model.Observacao);

            return new ApiResult(await _lancamentoServico.AlterarLancamento(idLancamento, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de um lançamento.
        /// </summary>
        [HttpDelete]
        [Route("lancamento/excluir")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Lançamento excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirLancamentoResponseExemplo))]
        public async Task<ISaida> ExcluirLancamento([FromQuery, SwaggerParameter("ID do lançamento que deverá ser excluído.", Required = true)] int idLancamento)
        {
            return await _lancamentoServico.ExcluirLancamento(idLancamento, base.ObterIdUsuarioClaim());
        }

        /// <summary>
        /// Realiza a exclusão de um anexo.
        /// </summary>
        [HttpDelete]
        [Route("lancamento/excluir-anexo")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Anexo excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirAnexoResponseExemplo))]
        public async Task<IActionResult> ExcluirAnexo([FromQuery, SwaggerParameter("ID do anexo que deverá ser excluído.", Required = true)] int idAnexo)
        {
            return new ApiResult(await _lancamentoServico.ExcluirAnexo(idAnexo, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza a exclusão de um detalhe.
        /// </summary>
        [HttpDelete]
        [Route("lancamento/excluir-detalhe")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Detalhe excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirDetalheResponseExemplo))]
        public async Task<IActionResult> ExcluirDetalhe([FromQuery, SwaggerParameter("ID do detalhe que deverá ser excluído.", Required = true)] int idDetalhe)
        {
            return new ApiResult(await _lancamentoServico.ExcluirDetalhe(idDetalhe, base.ObterIdUsuarioClaim()));
        }
    }
}
