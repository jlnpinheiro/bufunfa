using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Web.Models
{
    public class RelatorioExtratoPorPeriodoSaida
    {
        public Conta Conta { get; set; }

        public Periodo Periodo { get; set; }

        public IEnumerable<Lancamento> Lancamentos { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public decimal ValorTotalCredito => this.Lancamentos?.Where(x => x.Categoria.ObterTipo() == TipoCategoria.Credito)?.Sum(x => x.Valor) ?? 0;

        public decimal ValorTotalDebito => this.Lancamentos?.Where(x => x.Categoria.ObterTipo() == TipoCategoria.Debito)?.Sum(x => x.Valor) * -1 ?? 0;

        public decimal ValorSaldoPeriodo => this.ValorTotalCredito + this.ValorTotalDebito;

        public string ObterTotalCreditoEmReais() => this.ValorTotalCredito.ToString("c2");

        public string ObterTotalDebitoEmReais() => this.ValorTotalDebito.ToString("c2");

        public string ObterSaldoPeriodoEmReais() => this.ValorSaldoPeriodo.ToString("c2");
    }
}
