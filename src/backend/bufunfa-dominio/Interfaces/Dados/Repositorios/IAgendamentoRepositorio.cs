using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de agendamentos
    /// </summary>
    public interface IAgendamentoRepositorio
    {
        /// <summary>
        /// Obtém um agendamento a partir do seu ID
        /// </summary>
        Task<Agendamento> ObterPorId(int idAgendamento);

        /// <summary>
        /// Obtém os agendamentos baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarAgendamentoEntrada procurarEntrada);

        /// <summary>
        /// Insere um novo agendamento
        /// </summary>
        Task Inserir(Agendamento agendamento);

        /// <summary>
        /// Atualiza as informações do agendamento
        /// </summary>
        void Atualizar(Agendamento agendamento);

        /// <summary>
        /// Deleta um agendamento
        /// </summary>
        void Deletar(Agendamento agendamento);

        /// <summary>
        /// Deleta todos os agendamentos relacionados a uma conta.
        /// </summary>
        void DeletarPorConta(int idConta);
    }
}
