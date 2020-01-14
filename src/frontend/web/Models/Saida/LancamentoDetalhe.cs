namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de um detalhamento do lançamento
    /// </summary>
    public class LancamentoDetalhe
    {
        /// <summary>
        /// ID do detalhe
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int IdLancamento { get; set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Observação
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Categoria do lançamento
        /// </summary>
        public Categoria Categoria { get; set; }

        public string ObterValorEmReais() => this.Categoria.ObterTipo() == TipoCategoria.Debito
           ? (this.Valor * -1).ToString("c2")
           : this.Valor.ToString("c2");
    }
}
