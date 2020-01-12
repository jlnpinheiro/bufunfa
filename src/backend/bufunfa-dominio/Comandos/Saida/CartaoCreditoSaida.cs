using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de saída para as informações de um cartão de crédito
    /// </summary>
    public class CartaoCreditoSaida
    {
        /// <summary>
        /// Id do cartão
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome para identificação do cartão
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal ValorLimite { get; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; }

        /// <summary>
        /// Valor do limite de crédito disponível
        /// </summary>
        public decimal? ValorLimiteDisponivel { get; }

        public CartaoCreditoSaida(CartaoCredito cartao, decimal? valorLimiteDisponivel = null)
        {
            if (cartao == null)
                return;

            this.Id                    = cartao.Id;
            this.Nome                  = cartao.Nome;
            this.ValorLimite           = cartao.ValorLimite;
            this.DiaVencimentoFatura   = cartao.DiaVencimentoFatura;
            this.ValorLimiteDisponivel = valorLimiteDisponivel;
        }

        public CartaoCreditoSaida(
            int id,
            string nome,
            decimal valorLimite,
            int diaVencimentoFatura,
            decimal? valorLimiteDisponivel = null)
        {
            Id                    = id;
            Nome                  = nome;
            ValorLimite           = valorLimite;
            DiaVencimentoFatura   = diaVencimentoFatura;
            ValorLimiteDisponivel = valorLimiteDisponivel;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
