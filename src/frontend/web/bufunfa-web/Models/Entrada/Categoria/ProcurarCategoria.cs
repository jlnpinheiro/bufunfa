namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para a procura de categorias
    /// </summary>
    public class ProcurarCategoria : BaseModel
    {
        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Id da categoria pai
        /// </summary>
        public int? IdCategoriaPai { get; set; }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Caminho da categoria
        /// </summary>
        public string Caminho { get; set; }
    }
}
