using JNogueira.Bufunfa.Dominio.Entidades;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    public interface IUsuarioRepositorio
    {
        /// <summary>
        /// Obtém um usuário a partir do seu e-mail e senha
        /// </summary>
        Task<Usuario> ObterPorEmailSenha(string email, string senha, bool habilitarTracking = false);
    }
}
