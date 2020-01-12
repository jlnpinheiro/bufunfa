namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma fatura de cartão de crédito
    /// </summary>
    public class Fatura
    {
        /// <summary>
        /// ID da fatura
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int IdCartaoCredito { get; private set; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; private set; }

        /// <summary>
        /// Mês/ano da fatura
        /// </summary>
        public string MesAno { get; private set; }

        /// <summary>
        /// Valor adicional de crédito da fatura
        /// </summary>
        public decimal? ValorAdicionalCredito { get; private set; }

        /// <summary>
        /// Observação relacionada ao adicional de crédito
        /// </summary>
        public string ObservacaoCredito { get; private set; }

        /// <summary>
        /// Valor adicional de débito da fatura
        /// </summary>
        public decimal? ValorAdicionalDebito { get; private set; }

        /// <summary>
        /// Observação relacionada ao adicional de débito
        /// </summary>
        public string ObservacaoDebito { get; private set; }

        /// <summary>
        /// Cartão de crédito associado a fatura
        /// </summary>
        public CartaoCredito CartaoCredito { get; private set; }

        /// <summary>
        /// Lançamento associado ao pagamento da fatura
        /// </summary>
        public Lancamento Lancamento { get; private set; }

        private Fatura()
        {

        }

        public Fatura(
            int idCartaoCredito,
            int idLancamento,
            int mes,
            int ano,
            decimal? valorAdicionalCredito,
            string observacaoCredito,
            decimal? valorAdicionalDebito,
            string observacaoDebito)
        {
            IdCartaoCredito       = idCartaoCredito;
            IdLancamento          = idLancamento;
            MesAno                = mes.ToString().PadLeft(2, '0') + ano;
            ValorAdicionalCredito = valorAdicionalCredito;
            ObservacaoCredito     = observacaoCredito;
            ValorAdicionalDebito  = valorAdicionalDebito;
            ObservacaoDebito      = observacaoDebito;
        }
        
        public override string ToString()
        {
            return this.CartaoCredito.Nome + " - Fatura " + this.MesAno;
        }
    }
}