using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Conta"
    /// </summary>
    public interface IContaServico
    {
        /// <summary>
        /// Obtém uma conta a partir do seu ID
        /// </summary>
        Task<ISaida> ObterContaPorId(int idConta, int idUsuario);

        /// <summary>
        /// Obtém as contas de um usuário.
        /// </summary>
        Task<ISaida> ObterContasPorUsuario(int idUsuario);

        /// <summary>
        /// Realiza o cadastro de uma nova conta.
        /// </summary>
        Task<ISaida> CadastrarConta(ContaEntrada entrada);

        /// <summary>
        /// Altera as informações da conta
        /// </summary>
        Task<ISaida> AlterarConta(int idConta, ContaEntrada entrada);

        /// <summary>
        /// Exclui uma conta
        /// </summary>
        Task<ISaida> ExcluirConta(int idConta, int idUsuario);

        /// <summary>
        /// Obtém a análise de um ativo
        /// </summary>
        Task<ISaida> ObterAnaliseAtivo(int idConta, int idUsuario, decimal? valorCotacao = null);

        /// <summary>
        /// Obtém a análise dos ativos de um usuário.
        /// </summary>
        Task<ISaida> ObterAnaliseAtivosPorUsuario(int idUsuario);

        /// <summary>
        /// Realiza uma transferência de valores entre duas contas
        /// </summary>
        Task<ISaida> RealizarTransferencia(TransferenciaEntrada entrada);
    }
}
