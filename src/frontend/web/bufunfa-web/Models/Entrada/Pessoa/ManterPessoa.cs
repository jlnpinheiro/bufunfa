namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de uma pessoa
    /// </summary>
    public class ManterPessoa : BaseModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; set; }
    }
}
