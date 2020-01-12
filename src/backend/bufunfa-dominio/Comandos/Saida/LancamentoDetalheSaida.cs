using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de um detalhe
    /// </summary>
    public class LancamentoDetalheSaida
    {
        /// <summary>
        /// ID do detalhe
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; }

        /// <summary>
        /// Valor
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Categoria do detalhe
        /// </summary>
        public CategoriaSaida Categoria { get; }

        public LancamentoDetalheSaida(LancamentoDetalhe detalhe)
        {
            if (detalhe == null)
                return;

            this.Id           = detalhe.Id;
            this.IdLancamento = detalhe.IdLancamento;
            this.Valor        = detalhe.Valor;
            this.Observacao   = detalhe.Observacao;
            this.Categoria    = detalhe.Categoria != null
                ? new CategoriaSaida(detalhe.Categoria)
                : null;
        }

        public LancamentoDetalheSaida(
            int id,
            int idLancamento,
            decimal valor,
            CategoriaSaida categoria,
            string observacao = null)
        {
            Id           = id;
            IdLancamento = idLancamento;
            Valor        = valor;
            Observacao   = observacao;
            Categoria    = categoria;
        }
    }
}
