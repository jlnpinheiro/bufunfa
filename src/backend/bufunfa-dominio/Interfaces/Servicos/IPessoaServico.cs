using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionádos a entidade "Pessoa"
    /// </summary>
    public interface IPessoaServico
    {
        /// <summary>
        /// Obtém uma pessoa a partir do seu ID
        /// </summary>
        Task<ISaida> ObterPessoaPorId(int idPessoa, int idUsuario);

        /// <summary>
        /// Obtém as pessoas baseadas nos parâmetros de procura
        /// </summary>
        Task<ISaida> ProcurarPessoas(ProcurarPessoaEntrada entrada);

        /// <summary>
        /// Realiza o cadastro de uma nova pessoa.
        /// </summary>
        Task<ISaida> CadastrarPessoa(PessoaEntrada entrada);

        /// <summary>
        /// Altera as informações de uma pessoa
        /// </summary>
        Task<ISaida> AlterarPessoa(int idPessoa, PessoaEntrada entrada);

        /// <summary>
        /// Exclui uma pessoa
        /// </summary>
        Task<ISaida> ExcluirPessoa(int idPessoa, int idUsuario);
    }
}
