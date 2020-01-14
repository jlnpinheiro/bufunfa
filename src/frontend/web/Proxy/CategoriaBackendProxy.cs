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
        /// Obtém todas as categorias de um usuário
        /// </summary>
        public async Task<Saida<IEnumerable<Categoria>>> ObterCategorias() => await _httpClientHelper.FazerRequest<Saida<IEnumerable<Categoria>>>("categoria/obter", MetodoHttp.GET);

        /// <summary>
        /// Realiza a procura por categorias
        /// </summary>
        public async Task<Saida<ResultadoProcura<Categoria>>> ProcurarCategorias(ProcurarCategoria entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<ResultadoProcura<Categoria>>>("categoria/procurar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Cadastra uma nova categoria
        /// </summary>
        public async Task<Saida<Categoria>> CadastrarCategoria(ManterCategoria entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Categoria>>("categoria/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera uma categoria
        /// </summary>
        public async Task<Saida<Categoria>> AlterarCategoria(ManterCategoria entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Categoria>>("categoria/alterar?idCategoria=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui uma categoria
        /// </summary>
        public async Task<Saida<Categoria>> ExcluirCategoria(int id) => await _httpClientHelper.FazerRequest<Saida<Categoria>>("categoria/excluir?idCategoria=" + id, MetodoHttp.DELETE);
    }
}
