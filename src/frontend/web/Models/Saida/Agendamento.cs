using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de um agendamento
    /// </summary>
    public class Agendamento
    {
        /// <summary>
        /// ID do agendamento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Tipo do método de pagamento
        /// </summary>
        public int CodigoTipoMetodoPagamento { get; set; }

        /// <summary>
        /// Descrição do tipo do método de pagamento
        /// </summary>
        public string DescricaoTipoMetodoPagamento { get; set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Conta associada ao agendamento
        /// </summary>
        public Conta Conta { get; set; }

        /// <summary>
        /// Cartão de crédito associado ao agendamento
        /// </summary>
        public CartaoCredito CartaoCredito { get; set; }

        /// <summary>
        /// Pessoa associada ao agendamento
        /// </summary>
        public Pessoa Pessoa { get; set; }

        /// <summary>
        /// Categoria do agendamento
        /// </summary>
        public Categoria Categoria { get; set; }

        /// <summary>
        /// Parcelas do agendamento
        /// </summary>
        public IEnumerable<Parcela> Parcelas { get; set; }

        /// <summary>
        /// Data de vencimento da próxima parcela aberta.
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataProximaParcelaAberta { get; set; }

        /// <summary>
        /// Valor da próxima parcela aberta
        /// </summary>
        public decimal? ValorProximaParcelaAberta { get; set; }

        /// <summary>
        /// Data de vencimento da última parcela aberta.
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataUltimaParcelaAberta { get; set; }

        /// <summary>
        /// Quantidade total de parcelas.
        /// </summary>
        public int QuantidadeParcelas { get; set; }

        /// <summary>
        /// Quantidade total de parcelas abertas.
        /// </summary>
        public int QuantidadeParcelasAbertas { get; set; }

        /// <summary>
        /// Quantidade total de parcelas fechadas.
        /// </summary>
        public int QuantidadeParcelasFechadas { get; set; }

        /// <summary>
        /// Indica se o agendamento foi concluído, isto é, o número total de parcelas é igual ao número de parcelas descartadas e lançadas.
        /// </summary>
        public bool Concluido { get; set; }

        /// <summary>
        /// Valor total do agendamento
        /// </summary>
        public decimal ValorTotal { get; set; }

        /// <summary>
        /// Percentual de conclusão do agendamento
        /// </summary>
        public decimal PercentualConclusao { get; set; }

        public string ObterValorProximaParcelaEmReais() => this.ValorProximaParcelaAberta.HasValue && this.Categoria.ObterTipo() == TipoCategoria.Debito
            ? (this.ValorProximaParcelaAberta.Value * -1).ToString("c2")
            : (this.ValorProximaParcelaAberta.HasValue && this.Categoria.ObterTipo() == TipoCategoria.Credito
                ? this.ValorProximaParcelaAberta.Value.ToString("c2")
                : string.Empty);

        public string ObterValorTotalEmReais() => this.Categoria.ObterTipo() == TipoCategoria.Debito
            ? (this.ValorTotal * -1).ToString("c2")
            : this.ValorTotal.ToString("c2");
    }
}
