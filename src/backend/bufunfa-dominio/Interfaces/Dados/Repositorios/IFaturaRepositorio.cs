using JNogueira.Bufunfa.Dominio.Entidades;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de faturas
    /// </summary>
    public interface IFaturaRepositorio
    {
        /// <summary>
        /// Obtém uma fatura a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Fatura> ObterPorId(int idFatura, bool habilitarTracking = false);

        /// <summary>
        /// Obtém uma fatura a partir do ID do lançamento
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Fatura> ObterPorLancamento(int idLancamento, bool habilitarTracking = false);

        /// <summary>
        /// Obtém uma fatura a partir do seu ID cartão de crédito, mês e ano da fatura
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Fatura> ObterPorCartaoCreditoMesAno(int idCartao, int mesFatura, int anoFatura, bool habilitarTracking = false);

        /// <summary>
        /// Insere uma nova fatura
        /// </summary>
        Task Inserir(Fatura fatura);

        /// <summary>
        /// Deleta uma fatura
        /// </summary>
        void Deletar(Fatura fatura);
    }
}
