namespace JNogueira.Bufunfa.Api.ViewModels
{
    public abstract class ProcurarViewModel
    {
        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; set; }

        /// <summary>
        /// Nome da propriedade que deverá ser utilizada para ordenação do resultado da pesquisa
        /// </summary>
        public string OrdenarPor { get; set; }

        /// <summary>
        /// Sentido da ordenação do resultado da pesquisa
        /// </summary>
        public string OrdenarSentido { get; set; }

        public ProcurarViewModel()
        {
            this.OrdenarSentido = "ASC";
        }
    }
}
