using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de lançamentos
    /// </summary>
    public interface ILancamentoRepositorio
    {
        /// <summary>
        /// Obtém um lançamento a partir do seu ID
        /// </summary>
        Task<Lancamento> ObterPorId(int idLancamento);

        /// <summary>
        /// Obtém os lançamentos relacionadas a uma transferência, a partir do ID da transferência
        /// </summary>
        Task<IEnumerable<Lancamento>> ObterPorIdTransferencia(string idTransferencia);

        /// <summary>
        /// Obtém os lançamentos de um usuário por período para uma determinada conta.
        /// </summary>
        Task<IEnumerable<Lancamento>> ObterPorPeriodo(int idConta, DateTime dataInicio, DateTime dataFim);

        /// <summary>
        /// Obtém os lançamentos de um usuário por período .
        /// </summary>
        Task<IEnumerable<Lancamento>> ObterPorPeriodo(DateTime dataInicio, DateTime dataFim, int idUsuario);

        /// <summary>
        /// Obtém os lançamentos baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarLancamentoEntrada procurarEntrada);

        /// <summary>
        /// Insere uma novo lançamento
        /// </summary>
        Task Inserir(Lancamento lancamento);

        /// <summary>
        /// Atualiza as informações do lançamento
        /// </summary>
        void Atualizar(Lancamento lancamento);

        /// <summary>
        /// Deleta um lançamento
        /// </summary>
        void Deletar(Lancamento lancamento);

        /// <summary>
        /// Deleta todos os lançamentos relacionados a uma conta.
        /// </summary>
        void DeletarPorConta(int idConta);
    }
}
