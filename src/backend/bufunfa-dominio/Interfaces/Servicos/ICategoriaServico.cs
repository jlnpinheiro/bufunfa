using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Categoria"
    /// </summary>
    public interface ICategoriaServico
    {
        /// <summary>
        /// Obtém uma categoria a partir do seu ID
        /// </summary>
        Task<ISaida> ObterCategoriaPorId(int idCategoria, int idUsuario);

        /// <summary>
        /// Obtém as categorias de um usuário.
        /// </summary>
        Task<ISaida> ObterCategoriasPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém as categorias baseados nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarCategorias(ProcurarCategoriaEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de uma nova categoria.
        /// </summary>
        Task<ISaida> CadastrarCategoria(CategoriaEntrada entrada);

        /// <summary>
        /// Altera as informações da categoria.
        /// </summary>
        Task<ISaida> AlterarCategoria(int idCategoria, CategoriaEntrada entrada);

        /// <summary>
        /// Exclui uma categoria.
        /// </summary>
        Task<ISaida> ExcluirCategoria(int idCategoria, int idUsuario);
    }
}