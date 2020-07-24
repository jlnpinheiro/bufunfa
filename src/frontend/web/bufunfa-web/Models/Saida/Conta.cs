namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de uma conta
    /// </summary>
    public class Conta
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Codigo do tipo da conta
        /// </summary>
        public int CodigoTipo { get; set; }

        /// <summary>
        /// Descricao do tipo da conta
        /// </summary>
        public string DescricaoTipo { get; set; }

        /// <summary>
        /// Valor do saldo inicial da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; set; }

        /// <summary>
        /// Nome da instituição ao qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; set; }

        /// <summary>
        /// Valor do saldo atual da conta
        /// </summary>
        public decimal? ValorSaldoAtual { get; set; }

        public string ObterSaldoInicialEmReais() => this.ValorSaldoInicial?.ToString("c2");

        public string ObterSaldoAtualEmReais() => this.ValorSaldoAtual?.ToString("c2");

        public string ObterCorPorTipoConta()
        {
            switch (this.CodigoTipo)
            {
                case (int)TipoConta.ContaCorrente: return "#30BBBB";
                case (int)TipoConta.Poupanca: return "#00C0EF";
                case (int)TipoConta.RendaFixa: return "#555299";
                case (int)TipoConta.RendaVariavel: return "#FF7701";
            }

            return string.Empty;
        }
    }

    /// <summary>
    /// Tipo da conta
    /// </summary>
    public enum TipoConta
    {
        ContaCorrente = 1,
        Poupanca      = 2,
        RendaFixa     = 3,
        RendaVariavel = 4,
        FII = 5
    }
}
