using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Proxy
{
    public partial class BackendProxy
    {
        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        public async Task<Saida<Periodo>> ObterPeriodoPorId(int id) => await _httpClientHelper.FazerRequest<Saida<Periodo>>("periodo/obter-por-id?idPeriodo=" + id, MetodoHttp.GET);

        /// <summary>
        /// Obtém um período que contenha uma determinada data
        /// </summary>
        public async Task<Saida<Periodo>> ObterPeriodoPorDataReferencia(DateTime dataReferencia) => await _httpClientHelper.FazerRequest<Saida<Periodo>>($"periodo/obter-por-data?data={dataReferencia.ToString("dd-MM-yyyy")}", MetodoHttp.GET);

        /// <summary>
        /// Realiza a procura por períodos
        /// </summary>
        public async Task<Saida<ResultadoProcura<Periodo>>> ProcurarPeriodos(ProcurarPeriodo entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<ResultadoProcura<Periodo>>>("periodo/procurar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Cadastra um novo período
        /// </summary>
        public async Task<Saida<Periodo>> CadastrarPeriodo(ManterPeriodo entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Periodo>>("periodo/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera um período
        /// </summary>
        public async Task<Saida<Periodo>> AlterarPeriodo(ManterPeriodo entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Periodo>>("periodo/alterar?idPeriodo=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui um período
        /// </summary>
        public async Task<Saida<Periodo>> ExcluirPeriodo(int id) => await _httpClientHelper.FazerRequest<Saida<Periodo>>("periodo/excluir?idPeriodo=" + id, MetodoHttp.DELETE);
    }
}
