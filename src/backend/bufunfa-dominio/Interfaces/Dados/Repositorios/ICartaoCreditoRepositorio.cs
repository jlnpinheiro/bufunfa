using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de cartões de crédito
    /// </summary>
    public interface ICartaoCreditoRepositorio
    {
        /// <summary>
        /// Obtém um cartão a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<CartaoCredito> ObterPorId(int idCartaoCredito);

        /// <summary>
        /// Obtém os cartões de um usuário.
        /// </summary>
        Task<IEnumerable<CartaoCredito>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Insere um novo cartão
        /// </summary>
        Task Inserir(CartaoCredito cartao);

        /// <summary>
        /// Atualiza as informações do cartão
        /// </summary>
        void Atualizar(CartaoCredito cartao);

        /// <summary>
        /// Deleta um cartão
        /// </summary>
        void Deletar(CartaoCredito cartao);

        /// <summary>
        /// Verifica se um determinado usuário possui um cartão com o nome informado
        /// </summary>
        Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idCartaoCredito = null);

        /// <summary>
        /// Verifica a existência de um cartão de crédito a partir do seu ID
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int idUsuario, int idCartaoCredito);
    }
}
