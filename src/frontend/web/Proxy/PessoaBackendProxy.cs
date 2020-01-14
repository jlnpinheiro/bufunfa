using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Proxy
{
    public partial class BackendProxy
    {
        /// <summary>
        /// Obtém uma pessoa a partir do seu ID
        /// </summary>
        public async Task<Saida<Pessoa>> ObterPessoaPorId(int id) => await _httpClientHelper.FazerRequest<Saida<Pessoa>>("pessoa/obter-por-id?idPessoa=" + id, MetodoHttp.GET);

        /// <summary>
        /// Realiza a procura por pessoas
        /// </summary>
        public async Task<Saida<ResultadoProcura<Pessoa>>> ProcurarPessoas(ProcurarPessoa entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<ResultadoProcura<Pessoa>>>("pessoa/procurar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Cadastra uma nova pessoa
        /// </summary>
        public async Task<Saida<Pessoa>> CadastrarPessoa(ManterPessoa entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Pessoa>>("pessoa/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera uma pessoa
        /// </summary>
        public async Task<Saida<Pessoa>> AlterarPessoa(ManterPessoa entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Pessoa>>("pessoa/alterar?idPessoa=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui uma pessoa
        /// </summary>
        public async Task<Saida<Pessoa>> ExcluirPessoa(int id) => await _httpClientHelper.FazerRequest<Saida<Pessoa>>("pessoa/excluir?idPessoa=" + id, MetodoHttp.DELETE);
    }
}
