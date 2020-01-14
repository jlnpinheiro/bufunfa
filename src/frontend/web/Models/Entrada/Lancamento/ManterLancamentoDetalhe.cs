namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de um detalhe de um lançamento
    /// </summary>
    public class ManterLancamentoDetalhe : BaseModel
    {
        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Observações
        /// </summary>
        public string Observacao { get; set; }
    }
}
