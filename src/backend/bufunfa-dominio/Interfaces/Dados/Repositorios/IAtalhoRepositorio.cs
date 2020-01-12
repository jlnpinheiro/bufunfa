using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de atalhos
    /// </summary>
    public interface IAtalhoRepositorio
    {
        /// <summary>
        /// Obtém um atalho a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Atalho> ObterPorId(int idAtalho, bool habilitarTracking = false);

        /// <summary>
        /// Obtém todos os atalhos de um usuário
        /// </summary>
        Task<IEnumerable<Atalho>> ObterPorUsuario(int idUsuario);
        
        /// <summary>
        /// Verifica se um determinado usuário possui um atalho com o título informado
        /// </summary>
        Task<bool> VerificarExistenciaPorTitulo(int idUsuario, string titulo, int? idAtalho = null);

        /// <summary>
        /// Verifica se um determinado usuário possui um atalho com a URL informada
        /// </summary>
        Task<bool> VerificarExistenciaPorUrl(int idUsuario, string url, int? idAtalho = null);

        /// <summary>
        /// Insere um novo atalho
        /// </summary>
        Task Inserir(Atalho atalho);

        /// <summary>
        /// Atualiza as informações do atalho
        /// </summary>
        void Atualizar(Atalho atalho);

        /// <summary>
        /// Deleta um atalho
        /// </summary>
        void Deletar(Atalho atalho);
    }
}
