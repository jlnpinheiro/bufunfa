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
        /// Obtém todas as conta de um usuário
        /// </summary>
        public async Task<Saida<IEnumerable<CartaoCredito>>> ObterCartoesCredito() => await _httpClientHelper.FazerRequest<Saida<IEnumerable<CartaoCredito>>>("cartao-credito/obter", MetodoHttp.GET);

        /// <summary>
        /// Obtém um cartão de crédito a partir do seu ID
        /// </summary>
        public async Task<Saida<CartaoCredito>> ObterCartaoCreditoPorId(int id) => await _httpClientHelper.FazerRequest<Saida<CartaoCredito>>("cartao-credito/obter-por-id?idCartaoCredito=" + id, MetodoHttp.GET);

        /// <summary>
        /// Cadastra um novo cartão de crédito
        /// </summary>
        public async Task<Saida<CartaoCredito>> CadastrarCartaoCredito(ManterCartaoCredito entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<CartaoCredito>>("cartao-credito/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera um cartão de crédito
        /// </summary>
        public async Task<Saida<CartaoCredito>> AlterarCartaoCredito(ManterCartaoCredito entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<CartaoCredito>>("cartao-credito/alterar?idCartaoCredito=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui um cartão de crédito
        /// </summary>
        public async Task<Saida<CartaoCredito>> ExcluirCartaoCredito(int id) => await _httpClientHelper.FazerRequest<Saida<CartaoCredito>>("cartao-credito/excluir?idCartaoCredito=" + id, MetodoHttp.DELETE);

        /// <summary>
        /// Obtém uma fatura do cartão de crédito, a partir do mês e ano da fatura
        /// </summary>
        public async Task<Saida<Fatura>> ObterFaturaPorCartaoCredito(int idCartao, int mes, int ano) => await _httpClientHelper.FazerRequest<Saida<Fatura>>($"cartao-credito/obter-fatura?idCartaoCredito={idCartao}&mesFatura={mes}&anoFatura={ano}", MetodoHttp.GET);

        /// <summary>
        /// Obtém uma fatura do cartão de crédito, a partir do lançamento associado ao pagamento dessa fatura
        /// </summary>
        public async Task<Saida<Fatura>> ObterFaturaPorLancamento(int idLancamento) => await _httpClientHelper.FazerRequest<Saida<Fatura>>($"cartao-credito/obter-fatura-por-lancamento?idLancamento={idLancamento}", MetodoHttp.GET);

        /// <summary>
        /// Realiza o pagamento de uma fatura de cartão de crédito
        /// </summary>
        public async Task<Saida<Fatura>> PagarFatura(PagarFatura entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Fatura>>("cartao-credito/pagar-fatura", MetodoHttp.POST, content);
            }
        }
    }
}
