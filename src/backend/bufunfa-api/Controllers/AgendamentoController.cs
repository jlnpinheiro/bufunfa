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
    [SwaggerResponse((int)HttpStatusCode.BadRequest, "Notificações existentes. (Bad Request)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.BadRequest, typeof(BadRequestApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.NotFound, "Endereço não encontrado. (Not found)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.NotFound, typeof(NotFoundApiResponse))]
    [SwaggerResponse((int)HttpStatusCode.Unauthorized, "Acesso negado. Token de autenticação não encontrado. (Unauthorized)", typeof(Response))]
    [SwaggerResponseExample((int)HttpStatusCode.Unauthorized, typeof(UnauthorizedApiResponse))]
    [SwaggerTag("Permite a gestão e consulta dos agendamentos.")]
    public class AgendamentoController : BaseController
    {
        private readonly IAgendamentoServico _agendamentoServico;

        public AgendamentoController(IAgendamentoServico agendamentoServico)
        {
            _agendamentoServico = agendamentoServico;
        }

        /// <summary>
        /// Obtém um agendamento a partir do seu ID
        /// </summary>
        [HttpGet]
        [Route("agendamento/obter-por-id")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento encontrado.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterAgendamentoPorIdResponseExemplo))]
        public async Task<IActionResult> ObterAgendamentoPorId([FromQuery, SwaggerParameter("ID do agendamento.", Required = true)] int idAgendamento)
        {
            return new ApiResult(await _agendamentoServico.ObterAgendamentoPorId(idAgendamento, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Obtém as parcelas pertencentes a um determinado intervalo de tempo
        /// </summary>
        [HttpGet]
        [Route("agendamento/obter-parcelas-por-periodo")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcelas encontradas.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ObterParcelasPorPeriodoResponseExemplo))]
        public async Task<IActionResult> ObterParcelasPorPeriodo(
            [FromQuery, DateTimeModelBinder(DateFormat = "dd-MM-yyyy"), SwaggerParameter("Data inicial do período (formato dd-mm-aaaa).", Required = true)] DateTime? dataInicio,
            [FromQuery, DateTimeModelBinder(DateFormat = "dd-MM-yyyy"), SwaggerParameter("Data final do período (formato dd-mm-aaaa).", Required = true)] DateTime? dataFim,
            [FromQuery, SwaggerParameter("Indica que somente as parcelas abertas deverão ser obtidas.", Required = false)] bool somenteParcelasAbertas = true)
        {
            return new ApiResult(await _agendamentoServico.ObterParcelasPorPeriodo(dataInicio.Value, dataFim.Value, base.ObterIdUsuarioClaim(), somenteParcelasAbertas));
        }

        /// <summary>
        /// Realiza uma procura por agendamentos a partir dos parâmetros informados
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("agendamento/procurar")]
        [SwaggerRequestExample(typeof(ProcurarAgendamentoViewModel), typeof(ProcurarAgendamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Resultado da procura por agendamentos.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ProcurarAgendamentoResponseExemplo))]
        public async Task<IActionResult> Procurar([FromBody, SwaggerParameter("Parâmetros utilizados para realizar a procura.", Required = true)] ProcurarAgendamentoViewModel model)
        {
            var entrada = new ProcurarAgendamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdCategoria,
                model.IdConta,
                model.IdCartaoCredito,
                model.IdPessoa,
                model.DataInicioParcela,
                model.DataFimParcela,
                model.Concluido,
                model.OrdenarPor,
                model.OrdenarSentido,
                model.PaginaIndex,
                model.PaginaTamanho
            );

            return new ApiResult(await _agendamentoServico.ProcurarAgendamentos(entrada));
        }

        /// <summary>
        /// Realiza o cadastro de um novo agendamento.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("agendamento/cadastrar")]
        [SwaggerRequestExample(typeof(AgendamentoViewModel), typeof(AgendamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento cadastrado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarAgendamentoResponseExemplo))]
        public async Task<IActionResult> CadastrarAgendamento([FromBody, SwaggerParameter("Informações de cadastro do agendamento.", Required = true)] AgendamentoViewModel model)
        {
            var entrada = new AgendamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdCategoria.Value,
                model.IdConta,
                model.IdCartaoCredito,
                model.TipoMetodoPagamento,
                model.ValorParcela.Value,
                model.DataPrimeiraParcela.Value,
                model.QuantidadeParcelas.Value,
                model.PeriodicidadeParcelas,
                model.IdPessoa,
                model.Observacao);

            return new ApiResult(await _agendamentoServico.CadastrarAgendamento(entrada));
        }

        /// <summary>
        /// Realiza o cadastro de uma nova parcela.
        /// </summary>
        [HttpPost]
        [Consumes("application/json")]
        [Route("agendamento/cadastrar-parcela")]
        [SwaggerRequestExample(typeof(ParcelaViewModel), typeof(ParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela cadastrada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(CadastrarParcelaResponseExemplo))]
        public async Task<IActionResult> CadastrarParcela(
            [FromQuery, SwaggerParameter("ID do agendamento da parcela.", Required = true)] int idAgendamento,
            [FromBody, SwaggerParameter("Informações de cadastro da parcela.", Required = true)] ParcelaViewModel model)
        {
            var entrada = new ParcelaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Data.Value,
                model.Valor.Value,
                model.Observacao);

            return new ApiResult(await _agendamentoServico.CadastrarParcela(idAgendamento, entrada));
        }

        /// <summary>
        /// Realiza a alteração de um agendamento.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("agendamento/alterar")]
        [SwaggerRequestExample(typeof(AgendamentoViewModel), typeof(AgendamentoRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento alterado com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarAgendamentoResponseExemplo))]
        public async Task<IActionResult> AlterarAgendamento(
            [FromQuery, SwaggerParameter("ID do agendamento.", Required = true)] int idAgendamento,
            [FromBody, SwaggerParameter("Informações para alteração de um agendamento.", Required = true)] AgendamentoViewModel model)
        {
            var entrada = new AgendamentoEntrada(
                base.ObterIdUsuarioClaim(),
                model.IdCategoria.Value,
                model.IdConta,
                model.IdCartaoCredito,
                model.TipoMetodoPagamento,
                model.ValorParcela.Value,
                model.DataPrimeiraParcela.Value,
                model.QuantidadeParcelas.Value,
                model.PeriodicidadeParcelas,
                model.IdPessoa,
                model.Observacao);

            return new ApiResult(await _agendamentoServico.AlterarAgendamento(idAgendamento, entrada));
        }

        /// <summary>
        /// Realiza a alteração de uma parcela.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("agendamento/alterar-parcela")]
        [SwaggerRequestExample(typeof(ParcelaViewModel), typeof(AlterarParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela alterada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(AlterarParcelaResponseExemplo))]
        public async Task<IActionResult> AlterarParcela(
            [FromQuery, SwaggerParameter("ID da parcela.", Required = true)] int idParcela,
            [FromBody, SwaggerParameter("Informações para alteração de uma parcela.", Required = true)] ParcelaViewModel model)
        {
            var entrada = new ParcelaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Data.Value,
                model.Valor.Value,
                model.Observacao);

            return new ApiResult(await _agendamentoServico.AlterarParcela(idParcela, entrada));
        }

        /// <summary>
        /// Realiza o lançamento de uma parcela.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("agendamento/lancar-parcela")]
        [SwaggerRequestExample(typeof(LancarParcelaViewModel), typeof(LancarParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela lançada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(LancarParcelaResponseExemplo))]
        public async Task<IActionResult> LancarParcela(
            [FromQuery, SwaggerParameter("ID da parcela.", Required = true)] int idParcela,
            [FromBody, SwaggerParameter("Informações de lançamento da parcela.", Required = true)] LancarParcelaViewModel model)
        {
            var entrada = new LancarParcelaEntrada(
                base.ObterIdUsuarioClaim(),
                model.Data.Value,
                model.Valor.Value,
                model.Observacao);

            return new ApiResult(await _agendamentoServico.LancarParcela(idParcela, entrada));
        }

        /// <summary>
        /// Realiza o descarte de uma parcela.
        /// </summary>
        [HttpPut]
        [Consumes("application/json")]
        [Route("agendamento/descartar-parcela")]
        [SwaggerRequestExample(typeof(DescartarParcelaViewModel), typeof(DescartarParcelaRequestExemplo))]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela descartada com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(DescartarParcelaResponseExemplo))]
        public async Task<IActionResult> DescartarParcela(
            [FromQuery, SwaggerParameter("ID da parcela.", Required = true)] int idParcela,
            [FromBody, SwaggerParameter("Informações de descarte da parcela.", Required = true)] DescartarParcelaViewModel model)
        {
            var entrada = new DescartarParcelaEntrada(
                base.ObterIdUsuarioClaim(),
                model.MotivoDescarte);

            return new ApiResult(await _agendamentoServico.DescartarParcela(idParcela, entrada));
        }

        /// <summary>
        /// Realiza a exclusão de uma parcela.
        /// </summary>
        [HttpDelete]
        [Route("agendamento/excluir-parcela")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Parcela excluída com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirParcelaResponseExemplo))]
        public async Task<IActionResult> ExcluirParcela([FromQuery, SwaggerParameter("ID da parcela que deverá ser excluído.", Required = true)] int idParcela)
        {
            return new ApiResult(await _agendamentoServico.ExcluirParcela(idParcela, base.ObterIdUsuarioClaim()));
        }

        /// <summary>
        /// Realiza a exclusão de um agendamento.
        /// </summary>
        [HttpDelete]
        [Route("agendamento/excluir")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Agendamento excluído com sucesso.", typeof(Response))]
        [SwaggerResponseExample((int)HttpStatusCode.OK, typeof(ExcluirAgendamentoResponseExemplo))]
        public async Task<IActionResult> ExcluirAgendamento([FromQuery, SwaggerParameter("ID do agendamento que deverá ser excluído.", Required = true)] int idAgendamento)
        {
            return new ApiResult(await _agendamentoServico.ExcluirAgendamento(idAgendamento, base.ObterIdUsuarioClaim()));
        }
    }
}
