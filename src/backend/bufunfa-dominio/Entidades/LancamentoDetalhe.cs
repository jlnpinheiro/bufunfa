using JNogueira.Bufunfa.Dominio.Comandos;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um detalhe do lançamento
    /// </summary>
    public class LancamentoDetalhe
    {
        /// <summary>
        /// ID do detalhe
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do lançamento detalhado
        /// </summary>
        public int IdLancamento { get; private set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; private set; }

        /// <summary>
        /// Valor do detalhe
        /// </summary>
        public decimal Valor { get; private set; }

        /// <summary>
        /// Observação do detalhe
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Categoria associada ao detalhe
        /// </summary>
        public Categoria Categoria { get; private set; }

        /// <summary>
        /// Lançamento do detalhe
        /// </summary>
        public Lancamento Lancamento { get; private set; }

        private LancamentoDetalhe()
        {
        }

        public LancamentoDetalhe(int idLancamento, LancamentoDetalheEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdLancamento = idLancamento;
            this.IdCategoria  = entrada.IdCategoria;
            this.Valor        = entrada.Valor;
            this.Observacao   = entrada.Observacao;
        }

        public override string ToString()
        {
            return $"{this.Valor} - {this.Categoria.ObterCaminho()}";
        }
    }
}