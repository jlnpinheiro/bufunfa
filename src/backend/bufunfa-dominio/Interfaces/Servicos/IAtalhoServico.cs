using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Atalho"
    /// </summary>
    public interface IAtalhoServico
    {
        /// <summary>
        /// Obtém um atalho a partir do seu ID
        /// </summary>
        Task<ISaida> ObterAtalhoPorId(int idAtalho, int idUsuario);

        /// <summary>
        /// Obtém os atalhos de um usuário
        /// </summary>
        Task<ISaida> ObterAtalhosPorUsuario(int idUsuario);

        /// <summary>
        /// Realiza o cadastro de um atalho.
        /// </summary>
        Task<ISaida> CadastrarAtalho(AtalhoEntrada entrada);

        /// <summary>
        /// Altera as informações de um atalho.
        /// </summary>
        Task<ISaida> AlterarAtalho(int idAtalho, AtalhoEntrada entrada);

        /// <summary>
        /// Exclui um atalho.
        /// </summary>
        Task<ISaida> ExcluirAtalho(int idAtalho, int idUsuario);
    }
}
