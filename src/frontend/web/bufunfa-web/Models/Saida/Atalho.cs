namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de um atalho
    /// </summary>
    public class Atalho
    {
        /// <summary>
        /// ID do atalho
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Título do atalho
        /// </summary>
        public string Titulo { get; set; }

        /// <summary>
        /// URL do atalho
        /// </summary>
        public string Url { get; set; }
    }
}
