using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Período"
    /// </summary>
    public interface IPeriodoServico
    {
        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        Task<ISaida> ObterPeriodoPorId(int idPeriodo, int idUsuario);

        /// <summary>
        /// Obtém um período que compreenda a data informada
        /// </summary>
        Task<ISaida> ObterPeriodoPorData(DateTime data, int idUsuario);

        /// <summary>
        /// Obtém os períodos de um usuário.
        /// </summary>
        Task<ISaida> ObterPeriodosPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém os períodos baseados nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarPeriodos(ProcurarPeriodoEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de um novo período.
        /// </summary>
        Task<ISaida> CadastrarPeriodo(PeriodoEntrada entrada);

        /// <summary>
        /// Altera as informações do período
        /// </summary>
        Task<ISaida> AlterarPeriodo(int idPeriodo, PeriodoEntrada entrada);

        /// <summary>
        /// Exclui um período
        /// </summary>
        Task<ISaida> ExcluirPeriodo(int idPeriodo, int idUsuario);
    }
}
