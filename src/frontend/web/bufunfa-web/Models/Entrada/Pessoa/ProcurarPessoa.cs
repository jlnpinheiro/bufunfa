namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para a procura de pessoas
    /// </summary>
    public class ProcurarPessoa : BaseProcurar<PessoaOrdenarPor>
    {
        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; set; }
    }

    public enum PessoaOrdenarPor
    {
        Id,
        Nome
    }
}
