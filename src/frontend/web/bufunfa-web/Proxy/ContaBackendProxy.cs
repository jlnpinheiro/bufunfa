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
        /// Obtém uma conta a partir do seu ID
        /// </summary>
        public async Task<Saida<Conta>> ObterContaPorId(int id) => await _httpClientHelper.FazerRequest<Saida<Conta>>("conta/obter-por-id?idConta=" + id, MetodoHttp.GET);

        /// <summary>
        /// Obtém todas as conta de um usuário
        /// </summary>
        public async Task<Saida<IEnumerable<Conta>>> ObterContas() => await _httpClientHelper.FazerRequest<Saida<IEnumerable<Conta>>>("conta/obter", MetodoHttp.GET);

        /// <summary>
        /// Obtém a análise de uma determinado ativo
        /// </summary>
        public async Task<Saida<RendaVariavelAnalise>> ObterAnaliseAtivo(int id, decimal valorCotacao = 0) => await _httpClientHelper.FazerRequest<Saida<RendaVariavelAnalise>>($"conta/obter-analise-ativo?idAcao={id}&valorCotacao={valorCotacao.ToString("N2", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))}", MetodoHttp.GET);
        
        /// <summary>
        /// Obtém a análise dos ativos do usuário
        /// </summary>
        public async Task<Saida<IEnumerable<RendaVariavelAnalise>>> ObterAnaliseAtivos() => await _httpClientHelper.FazerRequest<Saida<IEnumerable<RendaVariavelAnalise>>>("conta/obter-analise-ativos", MetodoHttp.GET);

        /// <summary>
        /// Cadastra uma nova conta
        /// </summary>
        public async Task<Saida<Conta>> CadastrarConta(ManterConta entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Conta>>("conta/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera uma conta
        /// </summary>
        public async Task<Saida<Conta>> AlterarConta(ManterConta entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Conta>>("conta/alterar?idConta=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui uma conta
        /// </summary>
        public async Task<Saida<Conta>> ExcluirConta(int id) => await _httpClientHelper.FazerRequest<Saida<Conta>>("conta/excluir?idConta=" + id, MetodoHttp.DELETE);

        /// <summary>
        /// Realiza a transferência de valores entre contas
        /// </summary>
        public async Task<Saida<Transferencia>> RealizarTransferencia(Transferir entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Transferencia>>("conta/realizar-transferencia", MetodoHttp.POST, content);
            }
        }
    }
}
