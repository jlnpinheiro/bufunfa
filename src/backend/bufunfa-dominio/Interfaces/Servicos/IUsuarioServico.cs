using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Usuário"
    /// </summary>
    public interface IUsuarioServico
    {
        /// <summary>
        /// Realiza a autenticação de um usuário
        /// </summary>
        Task<ISaida> Autenticar(AutenticarUsuarioEntrada entrada);

        /// <summary>
        /// Realiza a troca da senha de acesso
        /// </summary>
        Task<ISaida> AlterarSenha(AlterarSenhaUsuarioEntrada entrada);
    }
}
