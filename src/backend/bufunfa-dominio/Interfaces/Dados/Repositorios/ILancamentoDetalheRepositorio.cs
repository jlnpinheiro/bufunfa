using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.NotifiqueMe;
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
        Task<LancamentoDetalhe> ObterPorId(int idDetalhe);

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
