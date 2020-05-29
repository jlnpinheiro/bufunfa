using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using JNogueira.Utilzao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("lancamentos")]
    public class LancamentoController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public LancamentoController(DatatablesHelper datatablesHelper, BackendProxy proxy)
            : base(proxy)
        {
            _datatablesHelper = datatablesHelper;
        }

        [HttpGet]
        [ExibirPeriodoAtualFilter]
        [ExibirAtalhosFilter]
        public async Task<IActionResult> Index([FromQuery]int id)
        {
            var contaSaida = await _proxy.ObterContaPorId(id);

            if (!contaSaida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, $"A conta com o ID {id} não existe.", null, TipoAcaoAoOcultarFeedback.RedirecionarTelaInicial));

            var conta = contaSaida.Retorno;

            var periodoSaida = await _proxy.ObterPeriodoPorDataReferencia(DateTime.Now.ConverterHorarioOficialBrasil());

            if (periodoSaida.Sucesso)
                ViewBag.PeriodoAtual = periodoSaida.Retorno;

            return View("Index", conta);
        }

        [HttpPost]
        [Route("listar-lancamentos")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de lançamentos cadastrados.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarLancamentos(ProcurarLancamento filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            LancamentoOrdenarPor ordenarPor;

            switch (_datatablesHelper.OrdenarPor)
            {
                case "categoria":
                    ordenarPor = LancamentoOrdenarPor.CategoriaCaminho;
                    break;
                case "pessoa":
                    ordenarPor = LancamentoOrdenarPor.NomePessoa;
                    break;
                default:
                    ordenarPor = LancamentoOrdenarPor.Data;
                    break;
            }

            filtro.OrdenarPor = ordenarPor;
            filtro.OrdenarSentido = _datatablesHelper.OrdenarSentido;
            filtro.PaginaIndex = _datatablesHelper.PaginaIndex;
            filtro.PaginaTamanho = _datatablesHelper.PaginaTamanho;

            var saida = await _proxy.ProcurarLancamentos(filtro);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a relação de lançamentos cadastrados.", saida.Mensagens));

            return new DatatablesResult(_datatablesHelper.Draw, saida.Retorno.TotalRegistros, saida.Retorno.Registros.Select(x => new {
                id              = x.Id,
                idParcela       = x.IdParcela,
                idAgendamento   = x.Parcela?.IdAgendamento,
                pagamentoFatura = x.Categoria.Id == 3, // Categoria referente ao pagamento de fatura
                data            = x.Data,
                pessoa          = x.Pessoa?.Nome,
                categoria       = x.Categoria.Caminho,
                idTransferencia = x.IdTransferencia,
                tipoCategoria   = x.Categoria.Tipo,
                valor           = x.Categoria.Tipo == "C" ? x.Valor : x.Valor * -1,
                observacao      = x.Observacao,
                anexos          = x.Anexos.Count(),
                detalhes        = x.Detalhes.Count()
            }).ToList());
        }

        [HttpGet]
        [Route("cadastrar-lancamento")]
        public IActionResult CadastrarLancamento(int? idConta)
        {
            Lancamento lancamento = null;

            if (idConta.HasValue)
                lancamento = new Lancamento { Conta = new Conta { Id = idConta.Value } };

            return PartialView("Manter", lancamento);
        }

        [HttpPost]
        [Route("cadastrar-lancamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarLancamento(ManterLancamento entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do lançamento não foram preenchidas.", new[] { "Verifique se todas as informações do lançamento foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            // Verifica se uma nova pessoa foi informado para o lançamento
            if (entrada.IdPessoa == 0 && !string.IsNullOrEmpty(entrada.NomePessoa))
            {
                var pessoaSaida = await _proxy.CadastrarPessoa(new ManterPessoa { Nome = entrada.NomePessoa });

                if (pessoaSaida.Sucesso)
                    entrada.IdPessoa = pessoaSaida.Retorno.Id;
                else
                    return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o lançamento.", pessoaSaida.Mensagens));
            }

            var saida = await _proxy.CadastrarLancamento(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o lançamento.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-lancamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações do lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarLancamento(int id)
        {
            var saida = await _proxy.ObterLancamentoPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações do lançamento.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-lancamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações do lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarLancamento(ManterLancamento entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do lançamento não foram preenchidas.", new[] { "Verifique se todas as informações do lançamento foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            // Verifica se uma nova pessoa foi informado para o lançamento
            if (entrada.IdPessoa == 0 && !string.IsNullOrEmpty(entrada.NomePessoa))
            {
                var pessoaSaida = await _proxy.CadastrarPessoa(new ManterPessoa { Nome = entrada.NomePessoa });

                if (pessoaSaida.Sucesso)
                    entrada.IdPessoa = pessoaSaida.Retorno.Id;
                else
                    return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o lançamento.", pessoaSaida.Mensagens));
            }

            var saida = await _proxy.AlterarLancamento(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar o lançamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-lancamento")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirLancamento(int id)
        {
            var saida = await _proxy.ExcluirLancamento(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o lançamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Lançamento excluído com sucesso."));
        }

        [HttpGet]
        [Route("exibir-anexos")]
        public async Task<IActionResult> ExibirAnexos(int idLancamento)
        {
            var saida = await _proxy.ObterLancamentoPorId(idLancamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir os anexos do lançamento.", saida.Mensagens));

            return PartialView("PopupAnexos", saida.Retorno);
        }

        [HttpGet]
        [Route("listar-anexos")]
        public async Task<IActionResult> ListarAnexo(int idLancamento)
        {
            var saida = await _proxy.ObterLancamentoPorId(idLancamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir os anexos do lançamento.", saida.Mensagens));

            return PartialView("ListarAnexos", saida.Retorno.Anexos);
        }

        [HttpGet]
        [Route("cadastrar-anexo")]
        public IActionResult CadastrarAnexo()
        {
            return PartialView("ManterAnexo");
        }

        [HttpGet]
        [Route("download-anexo")]
        public async Task<IActionResult> DownloadLancamentoAnexo(int id)
        {
            return await _proxy.RealizarDownloadLancamentoAnexo(id);
        }

        [HttpPost]
        [Route("cadastrar-anexo")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o anexo do lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarLancamentoAnexo(ManterLancamentoAnexo entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do detalhe do lançamento não foram preenchidas.", new[] { "Verifique se todas as informações do detalhe do lançamento foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarLancamentoAnexo(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o anexo do lançamento.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-anexo")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o anexo do lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirLancamentoAnexo(int id)
        {
            var saida = await _proxy.ExcluirLancamentoAnexo(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o anexo do lançamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Anexo do lançamento excluído com sucesso."));
        }


        [HttpGet]
        [Route("exibir-detalhes")]
        public async Task<IActionResult> ExibirDetalhes(int idLancamento)
        {
            var saida = await _proxy.ObterLancamentoPorId(idLancamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir os detalhes do lançamento.", saida.Mensagens));

            return PartialView("PopupDetalhes", saida.Retorno);
        }

        [HttpGet]
        [Route("listar-detalhes")]
        public async Task<IActionResult> ListarDetalhes(int idLancamento)
        {
            var saida = await _proxy.ObterLancamentoPorId(idLancamento);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir os detalhes do lançamento.", saida.Mensagens));

            return PartialView("ListarDetalhes", saida.Retorno.Detalhes);
        }

        [HttpGet]
        [Route("cadastrar-detalhe")]
        public IActionResult CadastrarDetalhe()
        {
            return PartialView("ManterDetalhe");
        }

        [HttpPost]
        [Route("cadastrar-detalhe")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar o detalhe do lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarLancamentoDetalhe(ManterLancamentoDetalhe entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações do detalhe do lançamento não foram preenchidas.", new[] { "Verifique se todas as informações do detalhe do lançamento foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarLancamentoDetalhe(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar o detalhe do lançamento.", saida.Mensagens));

            return new FeedbackResult(new Feedback(TipoFeedback.Sucesso, saida.Mensagens.First(), tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-detalhe")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir o detalhe do lançamento.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirLancamentoDetalhe(int id)
        {
            var saida = await _proxy.ExcluirLancamentoDetalhe(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível excluir o detalhe do lançamento.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Detalhe do lançamento excluído com sucesso."));
        }

        [HttpGet]
        [Route("cadastrar-operacao")]
        public async Task<IActionResult> CadastrarOperacao(int idConta)
        {
            var saida = await _proxy.ObterContaPorId(idConta);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter as informações da ação.", saida.Mensagens));

            ViewBag.Conta = saida.Retorno;

            return PartialView("ManterOperacao", null);
        }

        [HttpPost]
        [Route("cadastrar-operacao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao lançar a operação.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarOperacao(ManterLancamento entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da operação não foram preenchidas.", new[] { "Verifique se todas as informações da operação foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.CadastrarLancamento(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível cadastrar a operação.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Operação cadastrada com sucesso.", tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpGet]
        [Route("alterar-operacao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da operação.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarOperacao(int id)
        {
            var saida = await _proxy.ObterLancamentoPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da operação.", saida.Mensagens));

            ViewBag.Conta = saida.Retorno.Conta;

            return PartialView("ManterOperacao", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-operacao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao lançar a operação.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarOperacao(ManterLancamento entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da operação não foram preenchidas.", new[] { "Verifique se todas as informações da operação foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.AlterarLancamento(entrada);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível alterar a operação.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Operação alterado com sucesso.", tipoAcao: TipoAcaoAoOcultarFeedback.OcultarMoldais));
        }

        [HttpPost]
        [Route("excluir-operacao")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir a operação.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirOperacao(int id)
        {
            var saida = await _proxy.ExcluirLancamento(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a operação.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Operação excluída com sucesso."));
        }
    }

}
