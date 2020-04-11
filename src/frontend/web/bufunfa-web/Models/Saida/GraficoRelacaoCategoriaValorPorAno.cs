using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    public class GraficoRelacaoValorCategoriaPorAno
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; set; }

        /// <summary>
        /// Ano
        /// </summary>
        public int Ano { get; set; }

        /// <summary>
        /// Categoria
        /// </summary>
        public object Categoria { get; set; }

        /// <summary>
        /// Períodos pertencentes ao ano com seus respectivos lançamentos e parcelas
        /// </summary>
        public IEnumerable<PeriodoGraficoRelacaoValorCategoriaPorAno> Periodos { get; set; }

        /// <summary>
        /// Valor médio
        /// </summary>
        public decimal ValorMedio { get; set; }
    }

    public class PeriodoGrafico
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime DataInicio { get; set; }

        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime? DataInicioGrafico { get; set; }

        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime DataFim { get; set; }

        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy HH:mm:ss")]
        public DateTime? DataFimGrafico { get; set; }
    }

    public class PeriodoGraficoRelacaoValorCategoriaPorAno
    {
        /// <summary>
        /// Período
        /// </summary>
        public PeriodoGrafico Periodo { get; set; }

        /// <summary>
        /// Lançamentos
        /// </summary>
        public IEnumerable<LancamentoGraficoRelacaoValorCategoriaPorAno> Lancamentos { get; set; }

        /// <summary>
        /// Detlhes de lançamento
        /// </summary>
        public IEnumerable<LancamentoDetalheGraficoRelacaoValorCategoriaPorAno> LancamentoDetalhes { get; set; }

        // Parcelas
        public IEnumerable<ParcelaGraficoRelacaoValorCategoriaPorAno> Parcelas { get; set; }

        public decimal ValorTotal { get; set; }
    }

    public class LancamentoGraficoRelacaoValorCategoriaPorAno
    {
        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int Id { get; set; }

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
        /// Nome da conta
        /// </summary>
        public string NomeConta { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string NomePessoa { get; set; }

        /// <summary>
        /// ID da categoria
        /// </summary>
        public int IdCategoria { get; set; }

        public string Observacao { get; set; }
    }

    public class LancamentoDetalheGraficoRelacaoValorCategoriaPorAno
    {
        /// <summary>
        /// ID do detalhe
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime DataLancamento { get; set; }

        /// <summary>
        /// Conta
        /// </summary>
        public string NomeContaLancamento { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string NomePessoaLancamento { get; set; }

        /// <summary>
        /// Valor
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// ID da categoria
        /// </summary>
        public int IdCategoria { get; set; }

        public string Observacao { get; set; }
    }

    public class ParcelaGraficoRelacaoValorCategoriaPorAno
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
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? IdFatura { get; set; }

        /// <summary>
        /// Indica se a parcela já foi lançada
        /// </summary>
        public bool Lancada { get; set; }

        /// <summary>
        /// Data de lançamento da parcela
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataLancamento { get; set; }

        /// <summary>
        /// Agendamento da parcela
        /// </summary>
        public ParcelaAgendamento Agendamento { get; set; }
    }
}
