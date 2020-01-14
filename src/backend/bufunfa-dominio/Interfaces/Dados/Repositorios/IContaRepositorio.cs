using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de contas
    /// </summary>
    public interface IContaRepositorio
    {
        /// <summary>
        /// Obtém uma conta a partir do seu ID
        /// </summary>
        Task<Conta> ObterPorId(int idConta);

        /// <summary>
        /// Obtém as contas de um usuário.
        /// </summary>
        Task<IEnumerable<Conta>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Insere uma nova conta
        /// </summary>
        Task Inserir(Conta conta);

        /// <summary>
        /// Atualiza as informações da conta
        /// </summary>
        void Atualizar(Conta conta);

        /// <summary>
        /// Deleta uma conta
        /// </summary>
        void Deletar(Conta conta);

        /// <summary>
        /// Verifica se um determinado usuário possui uma conta com o nome informado
        /// </summary>
        Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idConta = null);

        /// <summary>
        /// Verifica a existência de uma conta a partir do seu ID
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int idUsuario, int idConta);
    }
}
