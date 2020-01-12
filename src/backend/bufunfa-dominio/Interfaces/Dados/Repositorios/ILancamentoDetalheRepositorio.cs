using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Infraestrutura.NotifiqueMe;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de detalhes do lançamento
    /// </summary>
    public interface ILancamentoDetalheRepositorio
    {
        /// <summary>
        /// Obtém um detalhe a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<LancamentoDetalhe> ObterPorId(int idDetalhe, bool habilitarTracking = false);

        /// <summary>
        /// Insere um novo detalhe no banco de dados.
        /// </summary>
        Task Inserir(LancamentoDetalhe detalhe);

        /// <summary>
        /// Deleta um detalhe
        /// </summary>
        void Deletar(LancamentoDetalhe detalhe);
    }
}
