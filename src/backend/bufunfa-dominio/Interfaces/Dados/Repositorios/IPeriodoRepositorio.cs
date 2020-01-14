using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de períodos
    /// </summary>
    public interface IPeriodoRepositorio
    {
        /// <summary>
        /// Obtém um período a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Periodo> ObterPorId(int idPeriodo);

        /// <summary>
        /// Obtém um período que compreenda a data informada
        /// </summary>
        Task<Periodo> ObterPorData(DateTime data, int idUsuario);

        /// <summary>
        /// Obtém os períodos de um usuário.
        /// </summary>
        Task<IEnumerable<Periodo>> ObterPorUsuario(int idUsuario);

        /// <summary>
        /// Obtém os períodos baseados nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarPeriodoEntrada procurarEntrada);

        /// <summary>
        /// Verifica se um determinado usuário possui um período com as datas de início e fim informados
        /// </summary>
        Task<bool> VerificarExistenciaPorDataInicioFim(int idUsuario, DateTime dataInicio, DateTime dataFim, int? idPeriodo = null);

        /// <summary>
        /// Insere um novo período
        /// </summary>
        Task Inserir(Periodo periodo);

        /// <summary>
        /// Atualiza as informações do período
        /// </summary>
        void Atualizar(Periodo periodo);

        /// <summary>
        /// Deleta um períoodo
        /// </summary>
        void Deletar(Periodo periodo);
    }
}
