using JNogueira.Bufunfa.Dominio.Comandos;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma conta (conta corrente, conta poupança, título público, etc)
    /// </summary>
    public class Conta
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Tipo da conta
        /// </summary>
        public TipoConta Tipo { get; private set; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; private set; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        public string NomeInstituicao { get; private set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        public string NumeroAgencia { get; private set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        public string Numero { get; private set; }

        private Conta()
        {

        }

        public Conta(ContaEntrada entrada)
            : this()
        {
            if (entrada.Invalido)
                return;

            this.IdUsuario         = entrada.IdUsuario;
            this.Nome              = entrada.Nome;
            this.Tipo              = entrada.Tipo;
            this.ValorSaldoInicial = entrada.ValorSaldoInicial.HasValue && entrada.ValorSaldoInicial.Value == 0 ? null : entrada.ValorSaldoInicial;
            this.NomeInstituicao   = entrada.NomeInstituicao;
            this.NumeroAgencia     = entrada.NumeroAgencia;
            this.Numero            = entrada.Numero;
        }

        public void Alterar(ContaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Nome              = entrada.Nome;
            this.Tipo              = entrada.Tipo;
            this.ValorSaldoInicial = entrada.ValorSaldoInicial;
            this.NomeInstituicao   = entrada.NomeInstituicao;
            this.NumeroAgencia     = entrada.NumeroAgencia;
            this.Numero            = entrada.Numero;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
