using Microsoft.AspNetCore.Http;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de um anexo de um lançamento
    /// </summary>
    public class ManterLancamentoAnexo : BaseModel
    {
        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; set; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        public string NomeArquivo { get; set; }

        /// <summary>
        /// Conteúdo do arquivo do anexo
        /// </summary>
        public IFormFile Arquivo { get; set; }
    }
}
