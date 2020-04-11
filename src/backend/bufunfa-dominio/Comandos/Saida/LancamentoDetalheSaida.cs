using JNogueira.Bufunfa.Dominio.Entidades;
using Newtonsoft.Json;
using System;


namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de um detalhe
    /// </summary>
    public class LancamentoDetalheSaida
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
        /// Valor
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Categoria do detalhe
        /// </summary>
        public CategoriaSaida Categoria { get; }

        /// <summary>
        /// Lancamento do detalhe
        /// </summary>
        public LancamentoDetalheLancamentoSaida Lancamento { get; set; }

        public LancamentoDetalheSaida(LancamentoDetalhe detalhe)
        {
            if (detalhe == null)
                return;

            this.Id = detalhe.Id;
            this.IdLancamento = detalhe.IdLancamento;
            this.Valor = detalhe.Valor;
            this.Observacao = detalhe.Observacao;
            this.Categoria = detalhe.Categoria != null
                ? new CategoriaSaida(detalhe.Categoria)
                : null;
            this.Lancamento = detalhe.Lancamento != null
                ? new LancamentoDetalheLancamentoSaida(detalhe.Lancamento)
                : null;
        }

        public LancamentoDetalheSaida(
            int id,
            int idLancamento,
            decimal valor,
            CategoriaSaida categoria,
            LancamentoSaida lancamento,
            string observacao = null)
        {
            Id = id;
            IdLancamento = idLancamento;
            Valor = valor;
            Observacao = observacao;
            Categoria = categoria;
            Lancamento = null;
        }
    }

    public class LancamentoDetalheLancamentoSaida
    {
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; }

        public ContaSaida Conta { get; }

        public PessoaSaida Pessoa { get; }

        public LancamentoDetalheLancamentoSaida(Lancamento lancamento)
        {
            this.Data = lancamento.Data;
            this.Conta = new ContaSaida(lancamento.Conta);
            this.Pessoa = new PessoaSaida(lancamento.Pessoa);
        }
    }
}
