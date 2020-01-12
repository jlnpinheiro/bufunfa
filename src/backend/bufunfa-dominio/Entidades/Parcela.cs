using JNogueira.Bufunfa.Dominio.Comandos;
using System;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma parcela
    /// </summary>
    public class Parcela
    {
        /// <summary>
        /// ID da parcela
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do agendamento que originou a parcela
        /// </summary>
        public int? IdAgendamento { get; private set; }

        /// <summary>
        /// Id da fatura cuja parcela pertence
        /// </summary>
        public int? IdFatura { get; private set; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        public DateTime Data { get; private set; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        public decimal Valor { get; private set; }

        /// <summary>
        /// Número da parcela
        /// </summary>
        public int Numero { get; private set; }

        /// <summary>
        /// Indica se a parcela já foi lançada
        /// </summary>
        public bool Lancada { get; private set; }

        /// <summary>
        /// Data do lançamento da parcela
        /// </summary>
        public DateTime? DataLancamento { get; private set; }

        /// <summary>
        /// Indica se a parcela já foi descartada
        /// </summary>
        public bool Descartada { get; private set; }

        /// <summary>
        /// Descrição do motivo de descarte da parcela
        /// </summary>
        public string MotivoDescarte { get; private set; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; private set; }

        /// <summary>
        /// Agendamento que originou a parcela
        /// </summary>
        public Agendamento Agendamento { get; private set; }

        private Parcela()
        {
            this.Lancada = false;
            this.Descartada = false;
        }

        public Parcela(int idAgendamento, ParcelaEntrada entrada)
            : this()
        {
            if (entrada.Invalido)
                return;

            this.IdAgendamento = idAgendamento;
            this.Data          = entrada.Data;
            this.Valor         = entrada.Valor;
            this.Observacao    = entrada.Observacao;
        }

        internal Parcela(
            DateTime data,
            decimal valor,
            int numero,
            string observacao = null)
        {
            Data       = data;
            Valor      = valor;
            Numero     = numero;
            Observacao = observacao;
        }

        public void Alterar(ParcelaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Data       = entrada.Data;
            this.Valor      = entrada.Valor;
            this.Observacao = entrada.Observacao;

            this.Agendamento?.AjustarNumeroParcelas();
        }

        public void Descartar(DescartarParcelaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Descartada = true;
            this.MotivoDescarte = entrada.MotivoDescarte;

            this.Agendamento?.AjustarNumeroParcelas();
        }

        public void Lancar(LancarParcelaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Lancada        = true;
            this.Valor          = entrada.Valor;
            this.DataLancamento = entrada.Data;
            this.Observacao     = entrada.Observacao;

            this.Agendamento?.AjustarNumeroParcelas();
        }

        public void DesfazerLancamento()
        {
            this.IdFatura = null;
            this.Lancada = false;
            this.DataLancamento = null;

            this.Agendamento?.AjustarNumeroParcelas();
        }

        public void PagarFatura(int idFatura, DateTime dataPagamento)
        {
            this.IdFatura       = idFatura;
            this.Lancada        = true;
            this.DataLancamento = dataPagamento;
        }

        public override string ToString()
        {
            return $"{this.Data.ToString("dd/MM/yyyy")} - {this.Valor.ToString("C2")}";
        }

        /// <summary>
        /// Indica a situação da parcela: fechada (quando lançada ou descartada) ou aberta.
        /// </summary>
        public StatusParcela ObterStatus()
        {
            return !this.Lancada && !this.Descartada
               ? StatusParcela.Aberta
               : StatusParcela.Fechada;
        }

        /// <summary>
        /// Ajusta o número da parcela
        /// </summary>
        internal void AjustarNumero(int numero)
        {
            this.Numero = numero;
        }
    }
}