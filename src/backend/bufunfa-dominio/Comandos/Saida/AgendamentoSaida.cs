using JNogueira.Bufunfa.Dominio.Entidades;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de um agendamento
    /// </summary>
    public class AgendamentoSaida
    {
        /// <summary>
        /// ID do agendamento
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Data de vencimento da próxima parcela aberta.
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataProximaParcelaAberta { get; }

        /// <summary>
        /// Valor da próxima parcela aberta
        /// </summary>
        public decimal? ValorProximaParcelaAberta { get; }

        /// <summary>
        /// Data de vencimento da última parcela aberta.
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataUltimaParcelaAberta { get; }

        /// <summary>
        /// Quantidade total de parcelas.
        /// </summary>
        public int QuantidadeParcelas { get; }

        /// <summary>
        /// Quantidade total de parcelas abertas.
        /// </summary>
        public int QuantidadeParcelasAbertas { get; }

        /// <summary>
        /// Quantidade total de parcelas lançadas.
        /// </summary>
        public int QuantidadeParcelasLancadas { get; }

        /// <summary>
        /// Quantidade total de parcelas descartadas.
        /// </summary>
        public int QuantidadeParcelasDescartadas { get; }

        /// <summary>
        /// Quantidade total de parcelas fechadas.
        /// </summary>
        public int QuantidadeParcelasFechadas { get; }

        /// <summary>
        /// Valor total do agendamento
        /// </summary>
        public decimal ValorTotal { get; }

        /// <summary>
        /// Percentual de conclusão do agendamento
        /// </summary>
        public decimal PercentualConclusao { get; }

        /// <summary>
        /// Indica se o agendamento foi concluído, isto é, o número total de parcelas é igual ao número de parcelas descartadas e lançadas.
        /// </summary>
        public bool Concluido { get; }

        /// <summary>
        /// Tipo do método de pagamento
        /// </summary>
        public MetodoPagamento CodigoTipoMetodoPagamento { get; }

        /// <summary>
        /// Descrição do tipo do método de pagamento
        /// </summary>
        public string DescricaoTipoMetodoPagamento { get; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Conta associada ao agendamento
        /// </summary>
        public ContaSaida Conta { get; }

        /// <summary>
        /// Cartão de crédito associado ao agendamento
        /// </summary>
        public CartaoCreditoSaida CartaoCredito { get; }

        /// <summary>
        /// Pessoa associada ao agendamento
        /// </summary>
        public PessoaSaida Pessoa { get; }

        /// <summary>
        /// Categoria do agendamento
        /// </summary>
        public CategoriaSaida Categoria { get; }

        /// <summary>
        /// Parcelas do agendamento
        /// </summary>
        public IEnumerable<ParcelaSaida> Parcelas { get; }        

        public AgendamentoSaida(Agendamento agendamento)
        {
            if (agendamento == null)
                return;

            this.Id                            = agendamento.Id;
            this.CodigoTipoMetodoPagamento     = agendamento.TipoMetodoPagamento;
            this.DescricaoTipoMetodoPagamento  = agendamento.TipoMetodoPagamento.ObterDescricao();
            this.Observacao                    = agendamento.Observacao;
            this.Conta                         = agendamento.IdConta.HasValue ? new ContaSaida(agendamento.Conta) : null;
            this.CartaoCredito                 = agendamento.IdCartaoCredito.HasValue ? new CartaoCreditoSaida(agendamento.CartaoCredito) : null;
            this.Pessoa                        = agendamento.IdPessoa.HasValue ? new PessoaSaida(agendamento.Pessoa) : null;
            this.Categoria                     = new CategoriaSaida(agendamento.Categoria);
            this.Parcelas                      = agendamento.Parcelas.Select(x => new ParcelaSaida(x));
            this.DataProximaParcelaAberta      = agendamento.ObterDataProximaParcelaAberta();
            this.ValorProximaParcelaAberta     = agendamento.ObterValorProximaParcelaAberta();
            this.DataUltimaParcelaAberta       = agendamento.ObterDataUltimaParcelaAberta();
            this.QuantidadeParcelas            = agendamento.ObterQuantidadeParcelas();
            this.QuantidadeParcelasAbertas     = agendamento.ObterQuantidadeParcelasAbertas();
            this.QuantidadeParcelasLancadas    = agendamento.ObterQuantidadeParcelasLancadas();
            this.QuantidadeParcelasDescartadas = agendamento.ObterQuantidadeParcelasDescartadas();
            this.QuantidadeParcelasFechadas    = agendamento.ObterQuantidadeParcelasFechadas();
            this.Concluido                     = agendamento.VerificarSeConcluido();
            this.ValorTotal                    = agendamento.ObterValorTotal();
            this.PercentualConclusao           = agendamento.ObterPercentualConclusao();
        }

        public AgendamentoSaida(
            int id,
            MetodoPagamento tipoMetodoPagamento,
            string observacao,
            ContaSaida conta,
            CartaoCreditoSaida cartaoCredito,
            PessoaSaida pessoa,
            CategoriaSaida categoria,
            IEnumerable<ParcelaSaida> parcelas,
            DateTime? dataProximaParcelaAberta,
            decimal? valorProximaParcelaAberta,
            DateTime? dataUltimaParcelaAberta,
            int quantidadeParcelas,
            int quantidadeParcelasAbertas,
            int quantidadeParcelasFechadas,
            bool concluido,
            decimal valorTotal,
            decimal percentualConclusao)
        {
            Id                            = id;
            CodigoTipoMetodoPagamento     = tipoMetodoPagamento;
            DescricaoTipoMetodoPagamento  = tipoMetodoPagamento.ObterDescricao();
            Observacao                    = observacao;
            Conta                         = conta;
            CartaoCredito                 = cartaoCredito;
            Pessoa                        = pessoa;
            Categoria                     = categoria;
            Parcelas                      = parcelas;
            DataProximaParcelaAberta      = dataProximaParcelaAberta;
            ValorProximaParcelaAberta     = valorProximaParcelaAberta;
            DataUltimaParcelaAberta       = dataUltimaParcelaAberta;
            QuantidadeParcelas            = quantidadeParcelas;
            QuantidadeParcelasAbertas     = quantidadeParcelasAbertas;
            QuantidadeParcelasLancadas    = 0;
            QuantidadeParcelasDescartadas = 0;
            QuantidadeParcelasFechadas    = quantidadeParcelasFechadas;
            Concluido                     = concluido;
            ValorTotal                    = valorTotal;
            PercentualConclusao           = percentualConclusao;
        }

        public override string ToString()
        {
            var descricao = new List<string>();

            if (this.Conta != null)
                descricao.Add(this.Conta.Nome);

            if (this.CartaoCredito != null)
                descricao.Add(this.CartaoCredito.Nome);

            if (!string.IsNullOrEmpty(this.Observacao))
                descricao.Add(this.Observacao);

            return string.Join(" » ", descricao);
        }
    }
}
