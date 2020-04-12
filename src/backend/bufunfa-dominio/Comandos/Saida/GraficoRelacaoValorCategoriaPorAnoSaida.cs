using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações que geram o gráfico que mostra valores para uma determinada categoria, por período em um determinado ano
    /// </summary>
    public class GraficoRelacaoValorCategoriaPorAnoSaida
    {
        /// <summary>
        /// ID do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Ano
        /// </summary>
        public int Ano { get; }

        /// <summary>
        /// Categoria
        /// </summary>
        public object Categoria { get; }

        /// <summary>
        /// Períodos pertencentes ao ano com seus respectivos lançamentos e parcelas
        /// </summary>
        public IEnumerable<PeriodoGraficoRelacaoValorCategoriaPorAnoSaida> Periodos { get; }

        /// <summary>
        /// Valor médio
        /// </summary>
        public decimal ValorMedio => this.Periodos.Sum(x => x.ValorTotal) / this.Periodos.Count();

        public GraficoRelacaoValorCategoriaPorAnoSaida(
            int idUsuario,
            int ano,
            CategoriaSaida categoria,
            IEnumerable<PeriodoGraficoRelacaoValorCategoriaPorAnoSaida> periodos)
        {
            this.IdUsuario = idUsuario;
            this.Ano       = ano;
            this.Categoria = new { categoria.Id, categoria.Tipo, categoria.Nome, categoria.Caminho };
            this.Periodos  = periodos;
        }
    }

    public class PeriodoGraficoRelacaoValorCategoriaPorAnoSaida
    {
        /// <summary>
        /// Período
        /// </summary>
        public object Periodo { get; }

        /// <summary>
        /// Lançamentos
        /// </summary>
        public IEnumerable<LancamentoGraficoRelacaoValorCategoriaPorAnoSaida> Lancamentos { get; }

        /// <summary>
        /// Detlhes de lançamento
        /// </summary>
        public IEnumerable<LancamentoDetalheGraficoRelacaoValorCategoriaPorAnoSaida> LancamentoDetalhes { get; }

        // Parcelas
        public IEnumerable<ParcelaGraficoRelacaoValorCategoriaPorAnoSaida> Parcelas { get; }

        public decimal ValorTotal => this.Lancamentos.Sum(x => x.Valor) + this.LancamentoDetalhes.Sum(x => x.Valor) + this.Parcelas.Sum(x => x.Valor);

        public PeriodoGraficoRelacaoValorCategoriaPorAnoSaida(
            PeriodoSaida periodo,
            IEnumerable<LancamentoSaida> lancamentos,
            IEnumerable<LancamentoDetalheSaida> lancamentoDetalhes,
            IEnumerable<ParcelaSaida> parcelas,
            DateTime? dataInicioPeriodoGrafico = null,
            DateTime? dataFimPeriodoGrafico = null)
        {
            this.Periodo = new 
            {
                periodo.Id,
                periodo.Nome,
                DataInicio = periodo.DataInicio.ToString("dd/MM/yyyy HH:mm:ss"),
                DataInicioGrafico = dataInicioPeriodoGrafico?.ToString("dd/MM/yyyy HH:mm:ss"),
                DataFim = periodo.DataFim.ToString("dd/MM/yyyy HH:mm:ss"),
                DataFimGrafico = dataFimPeriodoGrafico?.ToString("dd/MM/yyyy HH:mm:ss")
            };
            this.Lancamentos        = lancamentos?.Select(x => new LancamentoGraficoRelacaoValorCategoriaPorAnoSaida(x)) ?? new List<LancamentoGraficoRelacaoValorCategoriaPorAnoSaida>();
            this.LancamentoDetalhes = lancamentoDetalhes?.Select(x => new LancamentoDetalheGraficoRelacaoValorCategoriaPorAnoSaida(x)) ?? new List<LancamentoDetalheGraficoRelacaoValorCategoriaPorAnoSaida>();
            this.Parcelas           = parcelas.Select(x => new ParcelaGraficoRelacaoValorCategoriaPorAnoSaida(x)) ?? new List<ParcelaGraficoRelacaoValorCategoriaPorAnoSaida>();
        }
    }

    public class LancamentoGraficoRelacaoValorCategoriaPorAnoSaida
    {
        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Conta
        /// </summary>
        public string NomeConta { get; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string NomePessoa { get; }

        /// <summary>
        /// ID da categoria
        /// </summary>
        public int IdCategoria { get; }

        public string Observacao { get; }

        public LancamentoGraficoRelacaoValorCategoriaPorAnoSaida(LancamentoSaida lancamento)
        {
            this.Id          = lancamento.Id;
            this.Data        = lancamento.Data;
            this.Valor       = lancamento.Valor;
            this.NomeConta   = lancamento.Conta?.Nome;
            this.NomePessoa  = lancamento.Pessoa?.Nome;
            this.IdCategoria = lancamento.Categoria.Id;
            this.Observacao  = lancamento.Observacao;
        }
    }

    public class LancamentoDetalheGraficoRelacaoValorCategoriaPorAnoSaida
    {
        /// <summary>
        /// ID do detalhe
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime DataLancamento { get; }

        /// <summary>
        /// Conta
        /// </summary>
        public string NomeContaLancamento { get; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string NomePessoaLancamento { get; }

        /// <summary>
        /// Valor
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// ID da categoria
        /// </summary>
        public int IdCategoria { get; }

        public string Observacao { get; }

        public LancamentoDetalheGraficoRelacaoValorCategoriaPorAnoSaida(LancamentoDetalheSaida lancamentoDetalhe)
        {
            this.Id                   = lancamentoDetalhe.Id;
            this.IdLancamento         = lancamentoDetalhe.IdLancamento;
            this.DataLancamento       = lancamentoDetalhe.Lancamento.Data;
            this.NomeContaLancamento  = lancamentoDetalhe.Lancamento.Conta?.Nome;
            this.NomePessoaLancamento = lancamentoDetalhe.Lancamento.Pessoa?.Nome;
            this.Valor                = lancamentoDetalhe.Valor;
            this.IdCategoria          = lancamentoDetalhe.Categoria.Id;
            this.Observacao           = lancamentoDetalhe.Observacao;
        }
    }

    public class ParcelaGraficoRelacaoValorCategoriaPorAnoSaida
    {
        /// <summary>
        /// Número de parcela
        /// </summary>
        public int Numero { get; }

        /// <summary>
        /// Id da parcela
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? IdFatura { get; }

        /// <summary>
        /// Indica se a parcela já foi lançada
        /// </summary>
        public bool Lancada { get; }

        /// <summary>
        /// Data de lançamento da parcela
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataLancamento { get; }

        /// <summary>
        /// Agendamento da parcela
        /// </summary>
        public object Agendamento { get; }

        public ParcelaGraficoRelacaoValorCategoriaPorAnoSaida(ParcelaSaida parcela)
        {
            this.Id             = parcela.Id;
            this.Numero         = parcela.Numero;
            this.Valor          = parcela.Valor;
            this.IdFatura       = parcela.IdFatura;
            this.Lancada        = parcela.Lancada;
            this.DataLancamento = parcela.DataLancamento;
            this.Agendamento    = parcela.Agendamento;
        }
    }
}
