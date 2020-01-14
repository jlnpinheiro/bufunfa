using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Proxy
{
    public partial class BackendProxy
    {
        /// <summary>
        /// Obtém um lançamento a partir do seu ID
        /// </summary>
        public async Task<Saida<Lancamento>> ObterLancamentoPorId(int id) => await _httpClientHelper.FazerRequest<Saida<Lancamento>>("lancamento/obter-por-id?idLancamento=" + id, MetodoHttp.GET);

        /// <summary>
        /// Realiza a procura por lançamentos
        /// </summary>
        public async Task<Saida<ResultadoProcura<Lancamento>>> ProcurarLancamentos(ProcurarLancamento entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<ResultadoProcura<Lancamento>>>("lancamento/procurar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Cadastra um novo lançamento
        /// </summary>
        public async Task<Saida<Lancamento>> CadastrarLancamento(ManterLancamento entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Lancamento>>("lancamento/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera um lançamento
        /// </summary>
        public async Task<Saida<Lancamento>> AlterarLancamento(ManterLancamento entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Lancamento>>("lancamento/alterar?idLancamento=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui um lançamento
        /// </summary>
        public async Task<Saida<Lancamento>> ExcluirLancamento(int id) => await _httpClientHelper.FazerRequest<Saida<Lancamento>>("lancamento/excluir?idLancamento=" + id, MetodoHttp.DELETE);

        /// <summary>
        /// Cadastra um novo anexo para o lançamento
        /// </summary>
        public async Task<Saida<LancamentoAnexo>> CadastrarLancamentoAnexo(ManterLancamentoAnexo entrada)
        {
            byte[] data;

            using (var br = new BinaryReader(entrada.Arquivo.OpenReadStream()))
                data = br.ReadBytes((int)entrada.Arquivo.OpenReadStream().Length);

            var bytes = new ByteArrayContent(data);

            using (var multiContent = new MultipartFormDataContent
            {
                { bytes, nameof(entrada.Arquivo), entrada.Arquivo.FileName },
                { new StringContent(entrada.Descricao), nameof(entrada.Descricao) },
                { new StringContent(entrada.IdLancamento.ToString()), nameof(entrada.IdLancamento) },
                { new StringContent(entrada.NomeArquivo), nameof(entrada.NomeArquivo) }
            })
            {
                return await _httpClientHelper.FazerRequest<Saida<LancamentoAnexo>>($"lancamento/cadastrar-anexo?idLancamento={entrada.IdLancamento}", MetodoHttp.POST, multiContent);
            }
        }

        /// <summary>
        /// Realiza o download o arquivo de um anexo do lançamento
        /// </summary>
        public async Task<IActionResult> RealizarDownloadLancamentoAnexo(int idAnexo)
        {
            var response = await _httpClientHelper.FazerRequest("lancamento/realizar-download-anexo-por-id?idAnexo=" + idAnexo, MetodoHttp.GET);

            return response.IsSuccessStatusCode
                ? (ActionResult)new FileContentResult(await response.Content.ReadAsByteArrayAsync(), response.Content.Headers.ContentType.MediaType) { FileDownloadName = response.Content.Headers.ContentDisposition.FileName }
                : new JsonResult(await response.Content.ReadAsAsync<object>());
         }

        /// <summary>
        /// Exclui um anexo do lançamento
        /// </summary>
        public async Task<Saida<LancamentoAnexo>> ExcluirLancamentoAnexo(int idAnexo) => await _httpClientHelper.FazerRequest<Saida<LancamentoAnexo>>("lancamento/excluir-anexo?idAnexo=" + idAnexo, MetodoHttp.DELETE);


        /// <summary>
        /// Cadastra um novo detalhe para o lançamento
        /// </summary>
        public async Task<Saida<LancamentoDetalhe>> CadastrarLancamentoDetalhe(ManterLancamentoDetalhe entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<LancamentoDetalhe>>("lancamento/cadastrar-detalhe?idLancamento=" + entrada.IdLancamento, MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Exclui um detalhe do lançamento
        /// </summary>
        public async Task<Saida<LancamentoDetalhe>> ExcluirLancamentoDetalhe(int idDetalhe) => await _httpClientHelper.FazerRequest<Saida<LancamentoDetalhe>>("lancamento/excluir-detalhe?idDetalhe=" + idDetalhe, MetodoHttp.DELETE);
    }
}
