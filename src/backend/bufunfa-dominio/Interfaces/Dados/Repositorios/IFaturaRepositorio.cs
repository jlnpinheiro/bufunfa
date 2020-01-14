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
        Task<Fatura> ObterPorId(int idFatura);

        /// <summary>
        /// Obtém uma fatura a partir do ID do lançamento
        /// </summary>
        Task<Fatura> ObterPorLancamento(int idLancamento);

        /// <summary>
        /// Obtém uma fatura a partir do seu ID cartão de crédito, mês e ano da fatura
        /// </summary>
        Task<Fatura> ObterPorCartaoCreditoMesAno(int idCartao, int mesFatura, int anoFatura);

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
