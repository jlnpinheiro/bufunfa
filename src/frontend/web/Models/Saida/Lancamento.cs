using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de um lançamento
    /// </summary>
    public class Lancamento
    {
        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ID da parcela vinculada ao lançamento
        /// </summary>
        public int? IdParcela { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Quantidade de ações
        /// </summary>
        public int? QuantidadeAcoes { get; set; }

        /// <summary>
        /// ID da transferência
        /// </summary>
        public string IdTransferencia { get; set; }

        /// <summary>
        /// Observação
        /// </summary>
        public string Observacao { get; set; }

        /// <summary>
        /// Conta associado ao lançamento
        /// </summary>
        public Conta Conta { get; set; }

        /// <summary>
        /// Pessoa associada ao lançamento
        /// </summary>
        public Pessoa Pessoa { get; set; }

        /// <summary>
        /// Categoria associada ao lançamento
        /// </summary>
        public Categoria Categoria { get; set; }

        /// <summary>
        /// Parcela associada ao lançamento
        /// </summary>
        public Parcela Parcela { get; set; }

        /// <summary>
        /// Coleção de anexos do lançamento
        /// </summary>
        public IEnumerable<LancamentoAnexo> Anexos { get; set; }

        /// <summary>
        /// Coleção de detalhes do lançamento
        /// </summary>
        public IEnumerable<LancamentoDetalhe> Detalhes { get; set; }

        public string ObterValorEmReais() => this.Categoria.ObterTipo() == TipoCategoria.Debito
            ? (this.Valor * -1).ToString("c2")
            : this.Valor.ToString("c2");
    }
}
