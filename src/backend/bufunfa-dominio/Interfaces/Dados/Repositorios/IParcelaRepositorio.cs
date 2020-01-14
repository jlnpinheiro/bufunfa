using JNogueira.Bufunfa.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de parcelas
    /// </summary>
    public interface IParcelaRepositorio
    {
        /// <summary>
        /// Obtém uma parcela a partir do seu ID
        /// </summary>
        Task<Parcela> ObterPorId(int idParcela);

        /// <summary>
        /// Obtém as parcelas não lançadas e não descartadas associadas a um determinado cartão de crédito.
        /// </summary>
        Task<IEnumerable<Parcela>> ObterPorCartaoCredito(int idCartaoCredito, DateTime? dataFatura = null);

        /// <summary>
        /// Obtém as parcelas associadas a uma determinado fatura de cartão de crédito.
        /// </summary>
        Task<IEnumerable<Parcela>> ObterPorFatura(int idFatura);

        /// <summary>
        /// Obtém as parcelas pertencentes a um determinado intervalo de tempo
        /// </summary>
        Task<IEnumerable<Parcela>> ObterPorPeriodo(DateTime dataInicio, DateTime dataFim, int idUsuario, bool somenteParcelasAbertas = true);

        /// <summary>
        /// Insere uma nova parcela
        /// </summary>
        Task Inserir(Parcela parcela);

        /// <summary>
        /// Insere várias parcelas
        /// </summary>
        Task Inserir(IEnumerable<Parcela> parcelas);

        /// <summary>
        /// Atualiza as informações da parcel
        /// </summary>
        void Atualizar(Parcela parcela);

        /// <summary>
        /// Deleta uma parcela
        /// </summary>
        void Deletar(Parcela parcela);

        /// <summary>
        /// Deleta várias parcelas
        /// </summary>
        void Deletar(IEnumerable<Parcela> parcelas);

        /// <summary>
        /// Verifica a existência de uma parcela a partir do seu ID
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int idUsuario, int idParcela);
    }
}
