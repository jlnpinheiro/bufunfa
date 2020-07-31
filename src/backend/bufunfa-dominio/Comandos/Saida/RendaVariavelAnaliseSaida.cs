using JNogueira.Bufunfa.Dominio.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de saída para as informações da análise de uma ação
    /// </summary>
    public class RendaVariavelAnaliseSaida
    {
        /// <summary>
        /// ID da conta
        /// </summary>
        public int IdConta { get; }

        /// <summary>
        /// Sigla do ativo
        /// </summary>
        public string SiglaAtivo { get; }

        /// <summary>
        /// Ranking do ativo
        /// </summary>
        public int? RankingAtivo { get; }

        /// <summary>
        /// Nome do ativo
        /// </summary>
        public string NomeAtivo { get; }

        /// <summary>
        /// Tipo do ativo
        /// </summary>
        public int CodigoTipo { get; }

        /// <summary>
        /// Descrição do tipo do ativo
        /// </summary>
        public string DescricaoTipo { get; }

        /// <summary>
        /// Quantidade de ações na carteira
        /// </summary>
        public int QuantidadeEmCarteira { get; }

        /// <summary>
        /// Quantidade de ações compradas
        /// </summary>
        public int QuantidadeComprada { get; }

        /// <summary>
        /// Valor gasto com a compra de ações
        /// </summary>
        public decimal ValorTotalCompra { get; }

        /// <summary>
        /// Quantidade de ações vendidas
        /// </summary>
        public int QuantidadeVendida { get; }

        /// <summary>
        /// Valor total obtido com a venda das ações
        /// </summary>
        public decimal ValorTotalVenda { get; }

        /// <summary>
        /// Valor total obtido com juros ou dividendos
        /// </summary>
        public decimal ValorTotalJurosDividendos { get; }

        /// <summary>
        /// Valor total gasto com impostos
        /// </summary>
        public decimal ValorTotalImpostos { get; }

        /// <summary>
        /// Valor total (ganho ou prejuízo)
        /// </summary>
        public decimal ValorGanhoPrejuizo { get; }
        
        /// <summary>
        /// Informações sobre a cotação atual da ação
        /// </summary>
        public RendaVariavelCotacaoSaida Cotacao { get; }

        /// <summary>
        /// Valor de mercado
        /// </summary>
        public decimal ValorMercado => Math.Round(this.QuantidadeEmCarteira * (this.Cotacao != null ? this.Cotacao.Preco : 0), 2);

        /// <summary>
        /// Valor da possibilidade de ganhos
        /// </summary>
        public decimal ValorPossibilidadeGanho => this.QuantidadeEmCarteira > 0 ? Math.Round(this.ValorGanhoPrejuizo + this.ValorMercado, 2) : 0;

        /// <summary>
        /// Valor percentual da possibilidade de ganhos
        /// </summary>
        public decimal PercentualPossibilidadeGanho => this.QuantidadeEmCarteira > 0 ? Math.Round(this.ValorPossibilidadeGanho * 100 / Math.Abs(this.ValorGanhoPrejuizo), 2) : 0;

        /// <summary>
        /// Valor médio de compra da ação
        /// </summary>
        public decimal? ValorMedioCompra => this.QuantidadeComprada > 0 ? Math.Round(this.ValorTotalCompra / this.QuantidadeComprada, 2) : 0;

        /// <summary>
        /// Valor médio de venda da ação
        /// </summary>
        public decimal? ValorMedioVenda => this.QuantidadeVendida > 0 ? Math.Round(this.ValorTotalVenda / this.QuantidadeVendida, 2) : 0;

        /// <summary>
        /// Operações de compra, venda, impostos e dividendos/juros
        /// </summary>
        public IEnumerable<RendaVariavelOperacaoSaida> Operacoes { get; }

        public RendaVariavelAnaliseSaida(ContaSaida acao, IEnumerable<Lancamento> operacoes, RendaVariavelCotacaoSaida cotacao)
        {
            this.IdConta = acao.Id;
            this.RankingAtivo = acao.Ranking;
            this.SiglaAtivo = acao.Nome;
            this.NomeAtivo = acao.NomeInstituicao;
            this.CodigoTipo = acao.CodigoTipo;
            this.DescricaoTipo = acao.DescricaoTipo;

            this.QuantidadeComprada = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.CompraAcoes).Sum(x => x.QtdRendaVariavel.HasValue ? x.QtdRendaVariavel.Value : 0);
            this.ValorTotalCompra = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.CompraAcoes).Sum(x => x.Valor);

            this.QuantidadeVendida = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.VendaAcoes).Sum(x => x.QtdRendaVariavel.HasValue ? x.QtdRendaVariavel.Value : 0);
            this.ValorTotalVenda = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.VendaAcoes).Sum(x => x.Valor);

            this.QuantidadeEmCarteira = this.QuantidadeComprada - this.QuantidadeVendida;

            this.ValorTotalJurosDividendos = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.JurosDividendos).Sum(x => x.Valor);

            this.ValorTotalImpostos = operacoes.Where(x => x.IdCategoria == (int)TipoCategoriaEspecial.Impostos).Sum(x => x.Valor);

            this.ValorGanhoPrejuizo = this.ValorTotalVenda + this.ValorTotalJurosDividendos - (this.ValorTotalCompra + this.ValorTotalImpostos);

            this.Cotacao = cotacao;

            this.Operacoes = operacoes.OrderBy(x => x.Data).Select(x => new RendaVariavelOperacaoSaida(x));
        }

        public RendaVariavelAnaliseSaida(
            string siglaAcao,
            string nomeAcao,
            TipoConta tipo,
            int quantidadeEmCarteira,
            int quantidadeComprada,
            decimal valorTotalCompra,
            int quantidadeVendida,
            decimal valorTotalVenda,
            decimal valorTotalJurosDividendos,
            decimal valorTotalImpostos,
            decimal valorGanhoPrejuizo,
            RendaVariavelCotacaoSaida cotacao,
            IEnumerable<RendaVariavelOperacaoSaida> operacoes,
            int? ranking = null)
        {
            SiglaAtivo                 = siglaAcao;
            NomeAtivo                  = nomeAcao;
            CodigoTipo                 = (int)tipo;
            DescricaoTipo              = tipo.ObterDescricao();
            QuantidadeEmCarteira       = quantidadeEmCarteira;
            QuantidadeComprada         = quantidadeComprada;
            ValorTotalCompra           = valorTotalCompra;
            QuantidadeVendida          = quantidadeVendida;
            ValorTotalVenda            = valorTotalVenda;
            ValorTotalJurosDividendos  = valorTotalJurosDividendos;
            ValorTotalImpostos         = valorTotalImpostos;
            ValorGanhoPrejuizo         = valorGanhoPrejuizo;
            Cotacao                    = cotacao;
            Operacoes                  = operacoes;
            RankingAtivo               = ranking;
        }
    }

    public class RendaVariavelOperacaoSaida
    {
        public int IdLancamento { get; }

        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; }

        public object TipoOperacao { get; }

        public int QuantidadeAcoes { get; }

        public decimal ValorTotal { get; }

        public decimal ValorPorAcao => this.QuantidadeAcoes > 0 ? this.ValorTotal / this.QuantidadeAcoes : 0;

        public string Observacao { get; }

        public RendaVariavelOperacaoSaida(Lancamento lancamento)
        {
            IdLancamento = lancamento.Id;
            Data         = lancamento.Data;
            TipoOperacao = new
            {
                IdTipo = lancamento.IdCategoria,
                CodigoTipo = lancamento.Categoria.Tipo,
                DescricaoTipo = lancamento.Categoria.Nome,
            };

            QuantidadeAcoes = lancamento.QtdRendaVariavel.HasValue
                ? lancamento.QtdRendaVariavel.Value
                : 0;

            ValorTotal      = lancamento.Valor;
            Observacao      = lancamento.Observacao;
        }

        public RendaVariavelOperacaoSaida(
            int idLancamento,
            DateTime data,
            object tipoOperacao,
            int quantidadeAcoes,
            decimal valorTotal,
            string observacao)
        {
            IdLancamento    = idLancamento;
            Data            = data;
            TipoOperacao    = tipoOperacao;
            QuantidadeAcoes = quantidadeAcoes;
            ValorTotal      = valorTotal;
            Observacao      = observacao;
        }
    }
}
