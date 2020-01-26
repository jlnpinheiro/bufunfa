using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de uma parcela de agendamento
    /// </summary>
    public class Parcela
    {
        /// <summary>
        /// Número de parcela
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Id da parcela
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int? IdAgendamento { get; set; }

        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? IdFatura { get; set; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Indica se a parcela já foi lançada
        /// </summary>
        public bool Lancada { get; set; }

        /// <summary>
        /// Data do lançamento da parcela
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataLancamento { get; set; }

        /// <summary>
        /// Indica se a parcela já foi descartada
        /// </summary>
        public bool Descartada { get; set; }

        /// <summary>
        /// Descrição do motivo de descarte da parcela
        /// </summary>
        public string MotivoDescarte { get; set; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Agendamento
        /// </summary>
        public ParcelaAgendamento Agendamento { get; set; }

        public string ObterValorEmReais() => this.Agendamento.ObterTipo() == TipoCategoria.Debito
            ? (this.Valor * -1).ToString("c2")
            : this.Valor.ToString("c2");

        public Cor ObterCor()
        {
            if (this.Lancada || this.Descartada)
                return Cor.Verde;

            if (this.Data > DateTime.Now)
                return Cor.Amarelo;

            return Cor.Vermelho;
        }
    }

    public class ParcelaAgendamento
    {
        /// <summary>
        /// ID do agendamento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Conta { get; set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartao { get; set; }

        /// <summary>
        /// Nome do cartão de crédito
        /// </summary>
        public string CartaoCredito { get; set; }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string CategoriaTipo { get; set; }

        /// <summary>
        /// Caminho da categoria
        /// </summary>
        public string CategoriaCaminho { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Pessoa { get; set; }

        /// <summary>
        /// Quantidade de parcelas
        /// </summary>
        public int QuantidadeParcelas { get; set; }

        /// <summary>
        /// Obtém o tipo da categoria
        /// </summary>
        public TipoCategoria ObterTipo() => this.CategoriaTipo.Equals("C", StringComparison.CurrentCultureIgnoreCase) ? TipoCategoria.Credito : TipoCategoria.Debito;
    }

    public enum Cor
    {
        Verde,
        Amarelo,
        Vermelho
    }
}
