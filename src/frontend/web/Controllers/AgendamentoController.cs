using JNogueira.Bufunfa.Web.Binders;
using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Models;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("agendamentos")]
    public class AgendamentoController : BaseController
    {
        public AgendamentoController(BackendProxy proxy)
            : base(proxy)
        {

        }

        [HttpGet]
        [ExibirPeriodoAtualFilter]
        [ExibirAtalhosFilter]
        public IActionResult Index()
        {
            return View("Index");
        }

        [HttpPost]
        [Route("listar-agendamentos")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de agendamentos cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarAgendamentos(ProcurarAgendamento filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.ProcurarAgendamentos(filtro);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a relação de agendamentos cadastrados.", saida.Mensagens));

            return PartialView("Listar", saida.Retorno.Registros);
        }

        [HttpGet]
        [Route("cadastrar-agendamento")]
        public IActionResult CadastrarAgendamento()
        {
            return PartialView("Manter", null);
        }

        [HttpPost]
        [Route("cadastrar-agendamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o novo agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarAgendamento(ManterAgendamento entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do agendamento não foram preenchidas.", new[] { "Verifique se todas as informações do agendamento foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            // Verifica se uma nova pessoa foi informado para o agendamento
            if (entrada.IdPessoa == 0 && !string.IsNullOrEmpty(entrada.NomePessoa))
            {
                var pessoaSaida = await _proxy.CadastrarPessoa(new ManterPessoa { Nome = entrada.NomePessoa });

                if (pessoaSaida.Sucesso)
                    entrada.IdPessoa = pessoaSaida.Retorno.Id;
                else
                    return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o agendamento.", pessoaSaida.Mensagens));
            }

            var saida = await _proxy.CadastrarAgendamento(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o agendamento.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-agendamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarAgendamento(int id)
        {
            var saida = await _proxy.ObterAgendamentoPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do agendamento.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-agendamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarAgendamento(ManterAgendamento entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do agendamento não foram preenchidas.", new[] { "Verifique se todas as informações do agendamento foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            // Verifica se uma nova pessoa foi informado para o agendamento
            if (entrada.IdPessoa == 0 && !string.IsNullOrEmpty(entrada.NomePessoa))
            {
                var pessoaSaida = await _proxy.CadastrarPessoa(new ManterPessoa { Nome = entrada.NomePessoa });

                if (pessoaSaida.Sucesso)
                    entrada.IdPessoa = pessoaSaida.Retorno.Id;
                else
                    return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o agendamento.", pessoaSaida.Mensagens));
            }

            var saida = await _proxy.AlterarAgendamento(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o agendamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-agendamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirAgendamento(int id)
        {
            var saida = await _proxy.ExcluirAgendamento(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o agendamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));
        }

        [HttpGet]
        [Route("listar-parcelas-por-agendamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de parcelas do agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarParcelasPorAgendamento(int idAgendamento)
        {
            var saida = await _proxy.ObterAgendamentoPorId(idAgendamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a relação de parcelas do agendamento.", saida.Mensagens));

            return PartialView("ListarParcelasPorAgendamento", saida.Retorno);
        }

        [HttpGet]
        [Route("listar-parcelas-por-conta")]
        public async Task<IActionResult> ListarParcelasPorConta([DateTimeModelBinder(DateFormat = "dd/MM/yyyy")] DateTime? dataInicio, [DateTimeModelBinder(DateFormat = "dd/MM/yyyy")] DateTime? dataFim, int idConta, bool somenteParcelasAbertas = true)
        {
            var saida = await _proxy.ObterParcelasPorPeriodo(dataInicio.Value, dataFim.Value, somenteParcelasAbertas);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, $"Não foi possível obter as parcelas do período {dataInicio.Value.ToString("dd/MM/yyyy")} até {dataFim.Value.ToString("dd/MM/yyyy")} para a conta.", saida.Mensagens));

            var parcelasPorConta = saida.Retorno != null ? saida.Retorno.Where(x => x.Agendamento.IdConta == idConta).ToList() : new List<Parcela>();

            return PartialView("ListarParcelasPorConta", parcelasPorConta);
        }

        [HttpGet]
        [Route("cadastrar-parcela")]
        public IActionResult CadastrarParcela()
        {
            return PartialView("ManterParcela", null);
        }

        [HttpPost]
        [Route("cadastrar-parcela")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar a nova parcela.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarParcela(ManterParcela entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da parcela não foram preenchidas.", new[] { "Verifique se todas as informações da parcela foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarParcela(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a parcela.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-parcela")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da parcela.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarParcela(int idAgendamento, int idParcela)
        {
            var saida = await _proxy.ObterAgendamentoPorId(idAgendamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível as informações da parcela.", saida.Mensagens));

            var parcela = saida.Retorno.Parcelas.First(x => x.Id == idParcela);

            return PartialView("ManterParcela", parcela);
        }

        [HttpPost]
        [Route("alterar-parcela")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações da parcela.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarParcela(ManterParcela entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da parcela não foram preenchidas.", new[] { "Verifique se todas as informações da parcela foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.AlterarParcela(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a parcela.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("descartar-parcela")]
        public IActionResult DescartarParcela(int idParcela)
        {
            ViewBag.IdParcela = idParcela;

            return PartialView("DescartarParcela");
        }

        [HttpPost]
        [Route("descartar-parcela")]
        [FeedbackExceptionFilter("Ocorreu um erro ao descartar a parcela do agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> DescartarParcela(DescartarParcela entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações não foram preenchidas.", new[] { "Verifique se todas as informações foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.DescartarParcela(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível descartar a parcela do agendamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("lancar-parcela")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da parcela.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> LancarParcela(int idAgendamento, int idParcela)
        {
            var saida = await _proxy.ObterAgendamentoPorId(idAgendamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível as informações da parcela.", saida.Mensagens));

            var parcela = saida.Retorno.Parcelas.First(x => x.Id == idParcela);

            return PartialView("LancarParcela", parcela);
        }

        [HttpPost]
        [Route("lancar-parcela")]
        [FeedbackExceptionFilter("Ocorreu um erro ao lançar a parcela do agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> LancarParcela(ManterParcela entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações não foram preenchidas.", new[] { "Verifique se todas as informações foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.LancarParcela(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível lançar a parcela do agendamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-parcela")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir a parcela do agendamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirParcela(int idParcela)
        {
            var saida = await _proxy.ExcluirParcela(idParcela);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir a parcela do agendamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));
        }

        [HttpGet]
        [Route("pagar-com")]
        public IActionResult PagarCom()
        {
            return PartialView("PagarCom");
        }

        [HttpPost]
        [Route("pagar-com")]
        [FeedbackExceptionFilter("Ocorreu um erro ao pagar.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> PagarCom(Pagar entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações não foram preenchidas.", new[] { "Verifique se todas as informações foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var pessoaSaida = await _proxy.ProcurarPessoas(new ProcurarPessoa
            {
                Nome = entrada.NomePessoa,
                OrdenarPor = "Nome",
                OrdenarSentido = "ASC",
                PaginaIndex = 1,
                PaginaTamanho = 10
            });

            if (!pessoaSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, $"Não foi possível procurar a pessoa com o nome \"{entrada.NomePessoa}\".", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            var pessoa = pessoaSaida.Retorno.Registros.FirstOrDefault(x => x.Nome.Equals(entrada.NomePessoa, StringComparison.InvariantCultureIgnoreCase));

            if (pessoa == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, $"A pessoa com o nome \"{entrada.NomePessoa}\" não foi encontrada.", new[] { $"Cadastre uma pessoa com o nome \"{entrada.NomePessoa}\" antes de realizar o pagamento." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var cartao = (await _proxy.ObterCartaoCreditoPorId(entrada.IdCartaoCredito)).Retorno;

            var dataVencimentoFatura = new DateTime(entrada.DataCompra.Year, entrada.DataCompra.Month, cartao.DiaVencimentoFatura);

            var dataProximoVencimentoFatura = dataVencimentoFatura.AddMonths(1);

            DateTime dataParcela = entrada.DataCompra >= dataVencimentoFatura && entrada.DataCompra <= dataProximoVencimentoFatura.AddDays(-3)
                ? dataProximoVencimentoFatura
                : dataProximoVencimentoFatura.AddMonths(1);

            var agendamentoEntrada = new ManterAgendamento
            {
                IdCartaoCredito       = entrada.IdCartaoCredito,
                IdCategoria           = entrada.IdCategoria,
                IdPessoa              = pessoa.Id,
                Observacao            = entrada.Observacao,
                PeriodicidadeParcelas = Periodicidade.Mensal,
                TipoMetodoPagamento   = MetodoPagamento.Debito,
                QuantidadeParcelas    = 1,
                ValorParcela          = entrada.ValorCompra,
                DataPrimeiraParcela   = dataParcela
            };
            
            var saida = await _proxy.CadastrarAgendamento(agendamentoEntrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, $"Não foi possível pagar com o {entrada.NomePessoa}.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, $"Pagamento com o {entrada.NomePessoa} realizado com sucesso.", tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }
    }
}
