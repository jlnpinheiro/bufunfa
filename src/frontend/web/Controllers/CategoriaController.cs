using JNogueira.Bufunfa.Web.Filters;
using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using JNogueira.Bufunfa.Web.Proxy;
using JNogueira.Bufunfa.Web.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Controllers
{
    [Authorize]
    [Route("categorias")]
    public class CategoriaController : BaseController
    {
        private readonly DatatablesHelper _datatablesHelper;

        public CategoriaController(DatatablesHelper datatablesHelper, BackendProxy proxy)
            : base(proxy)
        {
            _datatablesHelper = datatablesHelper;
        }

        [HttpGet]
        [ExibirPeriodoAtualFilter]
        [ExibirAtalhosFilter]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("listar-categorias")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter a relação de categorias cadastradas.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ListarCategorias(ProcurarCategoria filtro)
        {
            if (filtro == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações para a procura não foram preenchidas.", tipoAcao: TipoAcaoAoOcultarFeedback.Ocultar));

            var saida = await _proxy.ProcurarCategorias(filtro);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível obter a relação de categorias cadastradas.", saida.Mensagens));

            return PartialView("Listar", saida.Retorno.Registros);
        }

        [HttpGet]
        [Route("cadastrar-categoria")]
        public IActionResult CadastrarCategoria()
        {
            return PartialView("Manter", null);
        }

        [HttpPost]
        [Route("cadastrar-categoria")]
        [FeedbackExceptionFilter("Ocorreu um erro ao cadastrar a nova categoria.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> CadastrarCategoria(ManterCategoria entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da categoria não foram preenchidas.", new[] { "Verifique se todas as informações da categoria foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            switch (entrada.IdCategoriaPai)
            {
                case int.MaxValue:
                    entrada.Tipo = "C";
                    break;
                case int.MinValue:
                    entrada.Tipo = "D";
                    break;
            }

            if (entrada.IdCategoriaPai.HasValue && entrada.IdCategoriaPai.Value == int.MinValue || entrada.IdCategoriaPai.Value == int.MaxValue)
                entrada.IdCategoriaPai = null;

            var saida = await _proxy.CadastrarCategoria(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível cadastrar a categoria.", saida.Mensagens));

            return Json(saida.Retorno);
        }

        [HttpGet]
        [Route("alterar-categoria")]
        [FeedbackExceptionFilter("Ocorreu um erro ao obter as informações da categoria.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCategoria(int id)
        {
            var saida = await _proxy.ObterCartaoCreditoPorId(id);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível exibir as informações da categoria.", saida.Mensagens));

            return PartialView("Manter", saida.Retorno);
        }

        [HttpPost]
        [Route("alterar-categoria")]
        [FeedbackExceptionFilter("Ocorreu um erro ao alterar as informações da categoria.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> AlterarCategoria(ManterCategoria entrada)
        {
            if (entrada == null)
                return new FeedbackResult(new Feedback(TipoFeedback.Atencao, "As informações da categoria não foram preenchidas.", new[] { "Verifique se todas as informações da categoria foram preenchidas." }, TipoAcaoAoOcultarFeedback.Ocultar));

            switch (entrada.IdCategoriaPai)
            {
                case int.MaxValue:
                    entrada.Tipo = "C";
                    break;
                case int.MinValue:
                    entrada.Tipo = "D";
                    break;
            }

            if (entrada.IdCategoriaPai.HasValue && entrada.IdCategoriaPai.Value == int.MinValue || entrada.IdCategoriaPai.Value == int.MaxValue)
                entrada.IdCategoriaPai = null;

            var saida = await _proxy.AlterarCategoria(entrada);

            if (!saida.Sucesso)
                return new FeedbackResult(new Feedback(TipoFeedback.Erro, "Não foi possível alterar a categoria.", saida.Mensagens));

            return Json(saida.Retorno);
        }

        [HttpPost]
        [Route("excluir-categoria")]
        [FeedbackExceptionFilter("Ocorreu um erro ao excluir a categoria.", TipoAcaoAoOcultarFeedback.Ocultar)]
        public async Task<IActionResult> ExcluirCategoria(int id)
        {
            var saida = await _proxy.ExcluirCategoria(id);

            return !saida.Sucesso
                ? new FeedbackResult(new Feedback(TipoFeedback.Atencao, "Não foi possível excluir a categoria.", saida.Mensagens))
                : new FeedbackResult(new Feedback(TipoFeedback.Sucesso, "Categoria excluída com sucesso."));
        }
    }
}
