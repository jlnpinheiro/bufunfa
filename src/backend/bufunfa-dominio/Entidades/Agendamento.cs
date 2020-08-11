using JNogueira.Bufunfa.Dominio.Comandos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um agendamento
    /// </summary>
    public class Agendamento
    {
        /// <summary>
        /// Id do agendamento
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; private set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; private set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; private set; }
        
        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; private set; }

        /// <summary>
        /// Tipo do método de pagamento
        /// </summary>
        public MetodoPagamento TipoMetodoPagamento { get; private set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Conta associada ao agendamento
        /// </summary>
        public Conta Conta { get; private set; }

        /// <summary>
        /// Cartão de crédito associado ao agendamento
        /// </summary>
        public CartaoCredito CartaoCredito { get; private set; }

        /// <summary>
        /// Pessoa associada ao agendamento
        /// </summary>
        public Pessoa Pessoa { get; private set; }

        /// <summary>
        /// Categoria do agendamento
        /// </summary>
        public Categoria Categoria { get; private set; }

        /// <summary>
        /// Parcelas do agendamento
        /// </summary>
        public IEnumerable<Parcela> Parcelas { get; private set; }

        private Agendamento()
        {
            this.Parcelas = new List<Parcela>();
        }

        public Agendamento(AgendamentoEntrada entrada)
            : this()
        {
            if (entrada.Invalido)
                return;

            this.IdUsuario           = entrada.IdUsuario;
            this.IdCategoria         = entrada.IdCategoria;
            this.IdConta             = entrada.IdConta;
            this.IdCartaoCredito     = entrada.IdCartaoCredito;
            this.IdPessoa            = entrada.IdPessoa;
            this.TipoMetodoPagamento = entrada.TipoMetodoPagamento;
            this.Observacao          = entrada.Observacao;
            this.Parcelas            = this.CriarParcelas(
                entrada.QuantidadeParcelas,
                entrada.DataPrimeiraParcela,
                entrada.ValorParcela,
                entrada.PeriodicidadeParcelas,
                entrada.Observacao);
        }

        public void Alterar(AgendamentoEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdCategoria         = entrada.IdCategoria;
            this.IdConta             = entrada.IdConta;
            this.IdCartaoCredito     = entrada.IdCartaoCredito;
            this.IdPessoa            = entrada.IdPessoa;
            this.TipoMetodoPagamento = entrada.TipoMetodoPagamento;
            this.Observacao          = entrada.Observacao;

            var parcelas = new List<Parcela>();

            // Adiciona a parcelas fechadas
            parcelas.AddRange(this.Parcelas.Where(x => x.ObterStatus() == StatusParcela.Fechada));

            // Cria novas parcelas
            parcelas.AddRange(this.CriarParcelas(
                entrada.QuantidadeParcelas,
                entrada.DataPrimeiraParcela,
                entrada.ValorParcela,
                entrada.PeriodicidadeParcelas,
                entrada.Observacao));

            this.Parcelas = parcelas;

            this.AjustarNumeroParcelas();
        }

        public void RemoverParcela(int idParcela)
        {
            var parcelas = this.Parcelas.Where(x => x.Id != idParcela);

            this.Parcelas = parcelas.ToList();

            this.AjustarNumeroParcelas();
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

        /// <summary>
        /// Data de vencimento da próxima parcela aberta.
        /// </summary>
        public DateTime? ObterDataProximaParcelaAberta() => this.Parcelas?.Any() != true || !this.Parcelas.Any(x => x.ObterStatus() == StatusParcela.Aberta)
            ? null
            : (DateTime?)this.Parcelas.Where(x => x.ObterStatus() == StatusParcela.Aberta).Min(x => x.Data);

        /// <summary>
        /// Valor da próxima parcela aberta.
        /// </summary>
        public decimal? ObterValorProximaParcelaAberta() => this.Parcelas?.Any() != true || !this.Parcelas.Any(x => x.ObterStatus() == StatusParcela.Aberta)
            ? null
            : (decimal?)this.Parcelas.Where(x => x.ObterStatus() == StatusParcela.Aberta).OrderBy(x => x.Data).First().Valor;

        /// <summary>
        /// Data de vencimento da última parcela aberta.
        /// </summary>
        public DateTime? ObterDataUltimaParcelaAberta() => this.Parcelas?.Any() != true || !this.Parcelas.Any(x => x.ObterStatus() == StatusParcela.Aberta)
            ? null
            : (DateTime?)this.Parcelas.Where(x => x.ObterStatus() == StatusParcela.Aberta).Max(x => x.Data);

        /// <summary>
        /// Quantidade total de parcelas.
        /// </summary>
        public int ObterQuantidadeParcelas() => this.Parcelas?.Any() != true
            ? 0
            : this.Parcelas.Count();

        /// <summary>
        /// Quantidade total de parcelas abertas.
        /// </summary>
        public int ObterQuantidadeParcelasAbertas() => this.Parcelas?.Any() != true
            ? 0
            : this.Parcelas.Count(x => x.ObterStatus() == StatusParcela.Aberta);

        /// <summary>
        /// Quantidade total de parcelas lançadas.
        /// </summary>
        public int ObterQuantidadeParcelasLancadas() => this.Parcelas?.Any() != true
            ? 0
            : this.Parcelas.Count(x => x.Lancada);

        /// <summary>
        /// Quantidade total de parcelas descartadas.
        /// </summary>
        public int ObterQuantidadeParcelasDescartadas() => this.Parcelas?.Any() != true
            ? 0
            : this.Parcelas.Count(x => x.Descartada);

        /// <summary>
        /// Quantidade total de parcelas fechadas.
        /// </summary>
        public int ObterQuantidadeParcelasFechadas() => this.Parcelas?.Any() != true
            ? 0
            : this.Parcelas.Count(x => x.ObterStatus() == StatusParcela.Fechada);

        /// <summary>
        /// Indica se o agendamento foi concluído, isto é, o número total de parcelas é igual ao número de parcelas descartadas e lançadas.
        /// </summary>
        public bool VerificarSeConcluido() => this.Parcelas?.Any() != true
            ? true
            : this.ObterQuantidadeParcelas() == this.ObterQuantidadeParcelasFechadas();

        /// <summary>
        /// Valor total do agendamento
        /// </summary>
        public decimal ObterValorTotal() => this.Parcelas?.Any() != true
            ? 0
            : this.Parcelas.Sum(x => x.Valor);

        /// <summary>
        /// Percentual de conclusão do agendamento
        /// </summary>
        public decimal ObterPercentualConclusao() => this.Parcelas?.Any() != true
            ? 0
            : Math.Round((decimal)100 * ObterQuantidadeParcelasFechadas() / ObterQuantidadeParcelas(), 0);

        /// <summary>
        /// Cria as parcelas do agendamento
        /// </summary>
        /// <param name="quantidadeParcelas">Quantidade total de parcelas do agendamento</param>
        /// <param name="dataPrimeiraParcela">Data do vencimento da primeira parcela</param>
        /// <param name="valor">Valor da parcela</param>
        /// <param name="periodicidade">Periodicidade no lançamento das parcelas</param>
        /// <param name="observacao">Observação da parcela</param>
        private IEnumerable<Parcela> CriarParcelas(int quantidadeParcelas, DateTime dataPrimeiraParcela, decimal valor, Periodicidade periodicidade, string observacao)
        {
            var cont = 1;

            var parcelas = new List<Parcela>() { new Parcela(dataPrimeiraParcela, valor, cont, observacao) }; // Parcela 1

            var dataParcela = dataPrimeiraParcela;

            while(cont < quantidadeParcelas)
            {
                cont++;

                dataParcela = dataParcela.AddMonths((int)periodicidade);

                parcelas.Add(new Parcela(dataParcela, valor, cont, observacao));
            }

            return parcelas;
        }

        /// <summary>
        /// Ajusta o número das parcelas
        /// </summary>
        internal void AjustarNumeroParcelas()
        {
            if (this.Parcelas?.Any() != true)
                return;

            var cont = 1;

            foreach(var parcela in this.Parcelas.OrderBy(x => x.Data))
            {
                parcela.AjustarNumero(cont);

                cont++;
            }
        }
    }
}
