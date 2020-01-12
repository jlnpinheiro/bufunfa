using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "CartaoCredito"
    /// </summary>
    public interface ICartaoCreditoServico
    {
        /// <summary>
        /// Obtém um cartão a partir do seu ID
        /// </summary>
        Task<ISaida> ObterCartaoCreditoPorId(int idCartao, int idUsuario, bool calcularLimiteCreditoDisponivelAtual = true);

        /// <summary>
        /// Obtém os cartões de um usuário.
        /// </summary>
        Task<ISaida> ObterCartoesCreditoPorUsuario(int idUsuario, bool calcularLimiteCreditoDisponivelAtual = true);

        /// <summary>
        /// Realiza o cadastro de um novo cartão.
        /// </summary>
        Task<ISaida> CadastrarCartaoCredito(CartaoCreditoEntrada entrada);

        /// <summary>
        /// Altera as informações do cartão.
        /// </summary>
        Task<ISaida> AlterarCartaoCredito(int idCartaoCredito, CartaoCreditoEntrada entrada);

        /// <summary>
        /// Exclui um cartão
        /// </summary>
        Task<ISaida> ExcluirCartaoCredito(int idCartao, int idUsuario);

        /// <summary>
        /// Obtém a fatura de um cartão de crédito, a partir do mês e ano da fatura
        /// </summary>
        Task<ISaida> ObterFaturaPorCartaoCredito(int idCartao, int idUsuario, int mesFatura, int anoFatura);

        /// <summary>
        /// Obtém a fatura de um cartão de crédito, a partir do ID do lançamento responsável pelo pagamento da fatura
        /// </summary>
        Task<ISaida> ObterFaturaPorLancamento(int idLancamento, int idUsuario);

        /// <summary>
        /// Realiza o pagamento de uma fatura
        /// </summary>
        Task<ISaida> PagarFatura(PagarFaturaEntrada entrada);
    }
}
