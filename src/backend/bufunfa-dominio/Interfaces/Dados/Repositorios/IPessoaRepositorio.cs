using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de pessoas
    /// </summary>
    public interface IPessoaRepositorio
    {
        /// <summary>
        /// Obtém um pessoa a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<Pessoa> ObterPorId(int idPessoa, bool habilitarTracking = false);

        /// <summary>
        /// Obtém as pessoas baseadas nos parâmetros de procura
        /// </summary>
        Task<ProcurarSaida> Procurar(ProcurarPessoaEntrada procurarEntrada);

        /// <summary>
        /// Verifica se um determinado usuário possui uma pessoa com o nome informado
        /// </summary>
        Task<bool> VerificarExistenciaPorNome(int idUsuario, string nome, int? idPessoa = null);

        /// <summary>
        /// Verifica a existência de uma pessoa a partir do seu ID
        /// </summary>
        Task<bool> VerificarExistenciaPorId(int idUsuario, int idPessoa);

        /// <summary>
        /// Insere uma nova pessoa
        /// </summary>
        Task Inserir(Pessoa pessoa);

        /// <summary>
        /// Atualiza as informações da pessoa
        /// </summary>
        void Atualizar(Pessoa pessoa);

        /// <summary>
        /// Deleta uma pessoa
        /// </summary>
        void Deletar(Pessoa pessoa);
    }
}
