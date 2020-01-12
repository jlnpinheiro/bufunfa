using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Agendamento"
    /// </summary>
    public interface IAgendamentoServico
    {
        /// <summary>
        /// Obtém um agendamento a partir do seu ID
        /// </summary>
        Task<ISaida> ObterAgendamentoPorId(int idAgendamento, int idUsuario);

        /// <summary>
        /// Obtém os agendamentos baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarAgendamentos(ProcurarAgendamentoEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de um novo agendamento.
        /// </summary>
        Task<ISaida> CadastrarAgendamento(AgendamentoEntrada entrada);

        /// <summary>
        /// Altera as informações de um agendamento.
        /// </summary>
        Task<ISaida> AlterarAgendamento(int idAgendamento, AgendamentoEntrada entrada);

        /// <summary>
        /// Exclui um agendamento.
        /// </summary>
        Task<ISaida> ExcluirAgendamento(int idAgendamento, int idUsuario);

        /// <summary>
        /// Obtém as parcelas pertencentes a um determinado intervalo de tempo
        /// </summary>
        Task<ISaida> ObterParcelasPorPeriodo(DateTime dataInicio, DateTime dataFim, int idUsuario, bool somenteParcelasAbertas = true);

        /// <summary>
        /// Realiza o cadastro de uma nova parcela.
        /// </summary>
        Task<ISaida> CadastrarParcela(int idAgendamento, ParcelaEntrada entrada);

        /// <summary>
        /// Altera as informações de uma parcela.
        /// </summary>
        Task<ISaida> AlterarParcela(int idParcela, ParcelaEntrada entrada);

        /// <summary>
        /// Lança uma parcela.
        /// </summary>
        Task<ISaida> LancarParcela(int idParcela, LancarParcelaEntrada entrada);

        /// <summary>
        /// Descarta uma parcela.
        /// </summary>
        Task<ISaida> DescartarParcela(int idParcela, DescartarParcelaEntrada entrada);

        /// <summary>
        /// Exclui uma parcela.
        /// </summary>
        Task<ISaida> ExcluirParcela(int idParcela, int idUsuario);
    }
}