namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de uma categoria
    /// </summary>
    public class ManterCategoria : BaseModel
    {
        /// <summary>
        /// Id da categoria
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id da categoria pai
        /// </summary>
        public int? IdCategoriaPai { get; set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; set; }
    }
}
