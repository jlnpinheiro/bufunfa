namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de um anexo do lançamento
    /// </summary>
    public class LancamentoAnexo
    {
        /// <summary>
        /// ID do anexo
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int IdLancamento { get; set; }

        /// <summary>
        /// ID do anexo no Google Drive
        /// </summary>
        public string IdGoogleDrive { get; set; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do arquivo
        /// </summary>
        public string NomeArquivo { get; set; }
    }
}
