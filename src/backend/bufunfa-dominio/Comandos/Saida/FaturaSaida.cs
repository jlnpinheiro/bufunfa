using JNogueira.Bufunfa.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de uma fatura de cartão de crédito
    /// </summary>
    public class FaturaSaida
    {
        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? Id { get; }

        /// <summary>
        /// Cartão de crédito da fatura
        /// </summary>
        public CartaoCreditoSaida CartaoCredito { get; }

        /// <summary>
        /// Parcelas pertencentes a fatura
        /// </summary>
        public IEnumerable<ParcelaSaida> Parcelas { get; }

        /// <summary>
        /// Mês da fatura
        /// </summary>
        public int Mes { get; }

        /// <summary>
        /// Ano da fatura
        /// </summary>
        public int Ano { get; }

        /// <summary>
        /// Valor adicional creditado a fatura
        /// </summary>
        public decimal? ValorAdicionalCredito { get; }

        /// <summary>
        /// Observação sobre o valor adicional creditado a fatura
        /// </summary>
        public string ObservacaoCredito { get; }
        
        /// <summary>
        /// Valor adicional debitado a fatura
        /// </summary>
        public decimal? ValorAdicionalDebito { get; }

        /// <summary>
        /// Observação sobre o valor adicional debitado a fatura
        /// </summary>
        public string ObservacaoDebito { get; }

        /// <summary>
        /// Lançamento relacionado ao pagamento da fatura.
        /// </summary>
        public LancamentoSaida Lancamento { get; }

        /// <summary>
        /// Valor total da fatura
        /// </summary>
        public decimal ValorTotalParcelas
        {
            get
            {
                var totalParcelasDebito = this.Parcelas.Where(x => ((dynamic)x.Agendamento).CategoriaTipo == TipoCategoria.Debito).Sum(x => x.Valor);

                var totalParcelasCredito = this.Parcelas.Where(x => ((dynamic)x.Agendamento).CategoriaTipo == TipoCategoria.Credito).Sum(x => x.Valor);

                return totalParcelasCredito - totalParcelasDebito;
            }
        }

        /// <summary>
        /// Valor total adicional (crédito - débito)
        /// </summary>
        public decimal ValorTotalAdicional => (this.ValorAdicionalCredito.HasValue ? this.ValorAdicionalCredito.Value : 0) - (this.ValorAdicionalDebito.HasValue ? this.ValorAdicionalDebito.Value : 0);

        /// <summary>
        /// Valor total da fatura (com o acréscimo do valor adicional de crédito e substraindo o valor adicional de débito)
        /// </summary>
        public decimal ValorFatura => this.ValorTotalParcelas < 0 
            ? (this.ValorTotalParcelas * -1) - this.ValorTotalAdicional 
            : this.ValorTotalAdicional - this.ValorTotalAdicional;

        public FaturaSaida(Fatura fatura, IEnumerable<ParcelaSaida> parcelas)
        {
            if (fatura == null)
                return;

            this.Id                    = fatura.Id;
            this.CartaoCredito         = new CartaoCreditoSaida(fatura.CartaoCredito);
            this.Lancamento            = new LancamentoSaida(fatura.Lancamento);
            this.Parcelas              = parcelas;
            this.Mes                   = Convert.ToInt32(fatura.MesAno.Substring(0, 2));
            this.Ano                   = Convert.ToInt32(fatura.MesAno.Substring(2));
            this.ValorAdicionalCredito = fatura.ValorAdicionalCredito;
            this.ObservacaoCredito     = fatura.ObservacaoCredito;
            this.ValorAdicionalDebito  = fatura.ValorAdicionalDebito;
            this.ObservacaoDebito      = fatura.ObservacaoDebito;
        }

        public FaturaSaida(
            CartaoCreditoSaida cartaoCredito,
            IEnumerable<ParcelaSaida> parcelas,
            int mes,
            int ano,
            LancamentoSaida lancamento = null)
        {
            Id                    = null;
            CartaoCredito         = cartaoCredito;
            Parcelas              = parcelas;
            Mes                   = mes;
            Ano                   = ano;
            Lancamento            = lancamento;
        }
    }
}
