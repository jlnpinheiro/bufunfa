using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de uma conta
    /// </summary>
    public class ContaSaida
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Tipo da conta
        /// </summary>
        public int CodigoTipo { get; }

        /// <summary>
        /// Descrição do tipo da conta
        /// </summary>
        public string DescricaoTipo { get; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; }

        /// <summary>
        /// Ranking da conta
        /// </summary>
        public int? Ranking { get; }

        /// <summary>
        /// Valor do saldo atual
        /// </summary>
        public decimal? ValorSaldoAtual { get; }

        public ContaSaida(Conta conta, decimal? valorSaldoAtual = null)
        {
            if (conta == null)
                return;

            this.Id                = conta.Id;
            this.Nome              = conta.Nome;
            this.CodigoTipo        = (int)conta.Tipo;
            this.DescricaoTipo     = conta.Tipo.ObterDescricao();
            this.ValorSaldoInicial = conta.ValorSaldoInicial;
            this.NomeInstituicao   = conta.NomeInstituicao;
            this.NumeroAgencia     = conta.NumeroAgencia;
            this.Numero            = conta.Numero;
            this.Ranking           = conta.Ranking;
            this.ValorSaldoAtual   = valorSaldoAtual;
        }

        public ContaSaida(
            int id,
            string nome,
            TipoConta tipo,
            decimal? valorSaldoInicial,
            string nomeInstituicao,
            string numeroAgencia,
            string numero,
            decimal? valorSaldoAtual = null,
            int? ranking = null)
        {
            Id                = id;
            Nome              = nome;
            CodigoTipo        = (int)tipo;
            DescricaoTipo     = tipo.ObterDescricao();
            ValorSaldoInicial = valorSaldoInicial;
            NomeInstituicao   = nomeInstituicao;
            NumeroAgencia     = numeroAgencia;
            Numero            = numero;
            Ranking           = ranking;
            ValorSaldoAtual   = valorSaldoAtual;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
