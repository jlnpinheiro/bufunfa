using JNogueira.Bufunfa.Dominio.Comandos;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um lançamento
    /// </summary>
    public class Lancamento
    {
        /// <summary>
        /// ID do lançamento
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int IdConta { get; private set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; private set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; private set; }

        /// <summary>
        /// Id da parcela que ocasionou o lançamento
        /// </summary>
        public int? IdParcela { get; private set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime Data { get; private set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; private set; }

        /// <summary>
        /// Quantidade de ativos (quando renda variável)
        /// </summary>
        public int? QtdRendaVariavel { get; private set; }

        /// <summary>
        /// ID da transferência
        /// </summary>
        public string IdTransferencia { get; private set; }

        /// <summary>
        /// Observação do lançamento
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Conta associada ao lançamento
        /// </summary>
        public Conta Conta { get; private set; }

        /// <summary>
        /// Categoria associada ao lançamento
        /// </summary>
        public Categoria Categoria { get; private set; }

        /// <summary>
        /// Pessoa associada ao lançamento
        /// </summary>
        public Pessoa Pessoa { get; private set; }

        /// <summary>
        /// Parcela associada ao lançamento
        /// </summary>
        public Parcela Parcela { get; private set; }

        /// <summary>
        /// Anexos do lançamento
        /// </summary>
        public IEnumerable<LancamentoAnexo> Anexos { get; private set; }

        /// <summary>
        /// Detalhes do lançamento
        /// </summary>
        public IEnumerable<LancamentoDetalhe> Detalhes { get; private set; }

        private Lancamento()
        {
            this.Anexos   = new List<LancamentoAnexo>();
            this.Detalhes = new List<LancamentoDetalhe>();
        }

        public Lancamento(LancamentoEntrada entrada, string idTransferencia = null)
            : this()
        {
            if (entrada.Invalido)
                return;

            this.IdUsuario        = entrada.IdUsuario;
            this.IdConta          = entrada.IdConta;
            this.IdCategoria      = entrada.IdCategoria;
            this.IdParcela        = entrada.IdParcela;
            this.Data             = entrada.Data;
            this.Valor            = entrada.Valor;
            this.QtdRendaVariavel = entrada.QuantidadeAcoes;
            this.IdPessoa         = entrada.IdPessoa;
            this.Observacao       = entrada.Observacao;
            this.IdTransferencia  = idTransferencia;
        }

        public void Alterar(LancamentoEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdConta          = entrada.IdConta;
            this.IdCategoria      = entrada.IdCategoria;
            this.Data             = entrada.Data;
            this.Valor            = entrada.Valor;
            this.QtdRendaVariavel = entrada.QuantidadeAcoes;
            this.IdPessoa         = entrada.IdPessoa;
            this.Observacao       = entrada.Observacao;
        }

        public override string ToString()
        {
            var descricao = new List<string>();

            if (this.Conta != null)
                descricao.Add(this.Conta.Nome);

            if (!string.IsNullOrEmpty(this.Observacao))
                descricao.Add(this.Observacao);

            descricao.Add(this.Data.ToString("dd/MM/yyyy"));
            descricao.Add(this.Valor.ToString("C2"));

            return string.Join(" » ", descricao);
        }
    }
}