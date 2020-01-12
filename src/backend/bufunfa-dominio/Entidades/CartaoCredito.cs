using JNogueira.Bufunfa.Dominio.Comandos;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um cartão de crédito
    /// </summary>
    public class CartaoCredito
    {
        /// <summary>
        /// Id do cartão
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome para identificação do cartão
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal ValorLimite { get; private set; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; private set; }

        private CartaoCredito()
        {

        }

        public CartaoCredito(CartaoCreditoEntrada cadastrarEntrada)
            : this()
        {
            if (cadastrarEntrada.Invalido)
                return;

            this.IdUsuario           = cadastrarEntrada.IdUsuario;
            this.Nome                = cadastrarEntrada.Nome;
            this.ValorLimite         = cadastrarEntrada.ValorLimite;
            this.DiaVencimentoFatura = cadastrarEntrada.DiaVencimentoFatura;
        }

        public void Alterar(CartaoCreditoEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Nome                = entrada.Nome;
            this.ValorLimite         = entrada.ValorLimite;
            this.DiaVencimentoFatura = entrada.DiaVencimentoFatura;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
