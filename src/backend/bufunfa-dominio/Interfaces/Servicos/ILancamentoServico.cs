using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.IO;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionados a entidade "Lançamento"
    /// </summary>
    public interface ILancamentoServico
    {
        /// <summary>
        /// Obtém um lançamento a partir do seu ID
        /// </summary>
        Task<ISaida> ObterLancamentoPorId(int idLancamento, int idUsuario);

        /// <summary>
        /// Obtém os lançamentos baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarLancamentos(ProcurarLancamentoEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de um novo lançamento.
        /// </summary>
        Task<ISaida> CadastrarLancamento(LancamentoEntrada entrada);

        /// <summary>
        /// Altera as informações de um lançamento.
        /// </summary>
        Task<ISaida> AlterarLancamento(int idLancamento, LancamentoEntrada entrada);

        /// <summary>
        /// Exclui um lançamento.
        /// </summary>
        Task<ISaida> ExcluirLancamento(int idLancamento, int idUsuario);

        /// <summary>
        /// Obtém um anexo do lançamento a partir do seu ID.
        /// </summary>
        Task<ISaida> ObterAnexoPorId(int idAnexo, int idUsuario);
        
        /// <summary>
        /// Realiza o cadastro de um novo anexo para um lançamento.
        /// </summary>
        Task<ISaida> CadastrarAnexo(int idLancamento, LancamentoAnexoEntrada entrada);

        /// <summary>
        /// Exclui um anexo de um lançamento.
        /// </summary>
        Task<ISaida> ExcluirAnexo(int idAnexo, int idUsuario);

        /// <summary>
        /// Realiza o cadastro de um novo detalhe para um lançamento.
        /// </summary>
        Task<ISaida> CadastrarDetalhe(int idLancamento, LancamentoDetalheEntrada entrada);

        /// <summary>
        /// Exclui um detalhe de um lançamento.
        /// </summary>
        Task<ISaida> ExcluirDetalhe(int idDetalhe, int idUsuario);
    }
}
