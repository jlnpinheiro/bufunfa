using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de uma análise de carteira de uma ação
    /// </summary>
    public class RendaVariavelAnalise
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public int IdConta { get; set; }

        /// <summary>
        /// Sigla do ativo
        /// </summary>
        public string SiglaAtivo { get; set; }

        /// <summary>
        /// Nome do ativo
        /// </summary>
        public string NomeAtivo { get; set; }

        /// <summary>
        /// Tipo do ativo
        /// </summary>
        public int CodigoTipo { get; set; }

        /// <summary>
        /// Descrição do tipo do ativo
        /// </summary>
        public string DescricaoTipo { get; set; }

        /// <summary>
        /// Quantidade de ações na carteira
        /// </summary>
        public int QuantidadeEmCarteira { get; set; }

        /// <summary>
        /// Quantidade de ações compradas
        /// </summary>
        public int QuantidadeComprada { get; set; }

        /// <summary>
        /// Valor gasto com a compra de ações
        /// </summary>
        public decimal ValorTotalCompra { get; set; }

        /// <summary>
        /// Quantidade de ações vendidas
        /// </summary>
        public int QuantidadeVendida { get; set; }

        /// <summary>
        /// Valor total obtido com a venda das ações
        /// </summary>
        public decimal ValorTotalVenda { get; set; }

        /// <summary>
        /// Valor total obtido com juros ou dividendos
        /// </summary>
        public decimal ValorTotalJurosDividendos { get; set; }

        /// <summary>
        /// Valor total gasto com impostos
        /// </summary>
        public decimal ValorTotalImpostos { get; set; }

        /// <summary>
        /// Valor total (ganho ou prejuízo)
        /// </summary>
        public decimal ValorGanhoPrejuizo { get; set; }

        /// <summary>
        /// Informações sobre a cotação atual da ação
        /// </summary>
        public RendaVariavelCotacao Cotacao { get; set; }

        /// <summary>
        /// Valor de mercado
        /// </summary>
        public decimal ValorMercado { get; set; }

        /// <summary>
        /// Valor da possibilidade de ganhos
        /// </summary>
        public decimal ValorPossibilidadeGanho { get; set; }

        /// <summary>
        /// Valor percentual da possibilidade de ganhos
        /// </summary>
        public decimal PercentualPossibilidadeGanho { get; set; }

        /// <summary>
        /// Valor médio de compra da ação
        /// </summary>
        public decimal? ValorMedioCompra { get; set; }

        /// <summary>
        /// Valor médio de venda da ação
        /// </summary>
        public decimal? ValorMedioVenda { get; set; }

        /// <summary>
        /// Operações de compra, venda, impostos e dividendos/juros
        /// </summary>
        public IEnumerable<RendaVariavelOperacao> Operacoes { get; set; }

        public string ObterValorTotalCompraEmReais() => (this.ValorTotalCompra * -1).ToString("c2");

        public string ObterValorMedioCompraEmReais() => this.ValorMedioCompra?.ToString("c2");

        public string ObterValorTotalVendaEmReais() => this.ValorTotalVenda.ToString("c2");

        public string ObterValorMedioVendaEmReais() => this.ValorMedioVenda?.ToString("c2");

        public string ObterValorTotalImpostosEmReais() => (this.ValorTotalImpostos * -1).ToString("c2");

        public string ObterValorTotalJurosDividendosEmReais() => this.ValorTotalJurosDividendos.ToString("c2");

        public string ObterValorGanhoPrejuizoEmReais() => this.ValorGanhoPrejuizo.ToString("c2");

        public string ObterValorPossibilidadeGanhoEmReais() => this.ValorPossibilidadeGanho.ToString("c2");

        public string ObterValorMercadoEmReais() => this.ValorMercado.ToString("c2");
    }

    public class RendaVariavelOperacao
    {
        /// <summary>
        /// ID da operação (lançamento)
        /// </summary>
        public int IdLancamento { get; set; }

        /// <summary>
        /// Data da operação (lançamento)
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Tipo da operação
        /// </summary>
        public RendaVarialTipoOperacao TipoOperacao { get; set; }

        /// <summary>
        /// Quantidade de ações
        /// </summary>
        public int QuantidadeAcoes { get; set; }

        /// <summary>
        /// Valor total da operação (lançamento)
        /// </summary>
        public decimal ValorTotal { get; set; }

        /// <summary>
        /// Observação
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Valor por ação da operação
        /// </summary>
        public decimal ValorPorAcao { get; set; }

        public string ObterValorTotalEmReais() => this.TipoOperacao.CodigoTipo == "D"
            ? (this.ValorTotal * -1).ToString("c2")
            : this.ValorTotal.ToString("c2");
    }

    public class RendaVarialTipoOperacao
    {
        /// <summary>
        /// ID do tipo
        /// </summary>
        public int IdTipo { get; set; }

        /// <summary>
        /// Código do tipo
        /// </summary>
        public string CodigoTipo { get; set; }

        /// <summary>
        /// Descrição do tipo
        /// </summary>
        public string DescricaoTipo { get; set; }
    }

    public class RendaVariavelCotacao
    {
        /// <summary>
        /// Preço da cotação
        /// </summary>
        public decimal Preco { get; set; }

        /// <summary>
        /// Indicativo percentual de alta ou baixa
        /// </summary>
        public decimal PercentualMudanca { get; set; }

        /// <summary>
        /// Data da cotação
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? Data { get; set; }
    }
}
