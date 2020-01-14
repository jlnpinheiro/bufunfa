using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Proxy
{
    public partial class BackendProxy
    {
        /// <summary>
        /// Obtém um atalho a partir do seu ID
        /// </summary>
        public async Task<Saida<Atalho>> ObterAtalhoPorId(int id) => await _httpClientHelper.FazerRequest<Saida<Atalho>>("atalho/obter-por-id?idAtalho=" + id, MetodoHttp.GET);

        /// <summary>
        /// Obtém todos os atalhos de um usuário
        /// </summary>
        public async Task<Saida<IEnumerable<Atalho>>> ObterAtalhos() => await _httpClientHelper.FazerRequest<Saida<IEnumerable<Atalho>>>("atalho/obter", MetodoHttp.GET);

        /// <summary>
        /// Cadastra um novo atalho
        /// </summary>
        public async Task<Saida<Atalho>> CadastrarAtalho(ManterAtalho entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Atalho>>("atalho/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera um atalho
        /// </summary>
        public async Task<Saida<Atalho>> AlterarAtalho(ManterAtalho entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Atalho>>("atalho/alterar?idAtalho=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui um atalho
        /// </summary>
        public async Task<Saida<Atalho>> ExcluirAtalho(int id) => await _httpClientHelper.FazerRequest<Saida<Atalho>>("atalho/excluir?idAtalho=" + id, MetodoHttp.DELETE);
    }
}
