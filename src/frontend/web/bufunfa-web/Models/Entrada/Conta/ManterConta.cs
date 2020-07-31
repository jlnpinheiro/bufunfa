namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de uma conta
    /// </summary>
    public class ManterConta : BaseModel
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo da conta
        /// </summary>
        public int Tipo { get; set; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; set; }

        /// <summary>
        /// Nome da instituição financeira
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
        /// Ranking da conta
        /// </summary>
        public int? Ranking { get; set; }
    }
}
