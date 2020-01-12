using JNogueira.Bufunfa.Dominio.Entidades;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public class ParcelaSaida
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
        /// Id do agendamento
        /// </summary>
        public int? IdAgendamento { get; }

        /// <summary>
        /// Id da fatura
        /// </summary>
        public int? IdFatura { get; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime Data { get; }

        /// <summary>
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; }

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
        /// Indica se a parcela já foi descartada
        /// </summary>
        public bool Descartada { get; }

        /// <summary>
        /// Descrição do motivo de descarte da parcela
        /// </summary>
        public string MotivoDescarte { get; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; }

        /// <summary>
        /// Agendamento
        /// </summary>
        public object Agendamento { get; }

        public ParcelaSaida(Parcela parcela)
        {
            if (parcela == null)
                return;

            this.Id             = parcela.Id;
            this.IdAgendamento  = parcela.IdAgendamento;
            this.IdFatura       = parcela.IdFatura;
            this.Data           = parcela.Data;
            this.Valor          = parcela.Valor;
            this.Numero         = parcela.Numero;
            this.Lancada        = parcela.Lancada;
            this.DataLancamento = parcela.DataLancamento;
            this.Descartada     = parcela.Descartada;
            this.MotivoDescarte = parcela.MotivoDescarte;
            this.Observacao     = parcela.Observacao;
            this.Agendamento = parcela.Agendamento != null
                ? new
                {
                    parcela.Agendamento.Id,
                    IdConta = parcela.Agendamento.Conta?.Id,
                    IdCartao = parcela.Agendamento.CartaoCredito?.Id,
                    Conta = parcela.Agendamento.Conta?.Nome,
                    CartaoCredito = parcela.Agendamento.CartaoCredito?.Nome,
                    CategoriaTipo = parcela.Agendamento.Categoria.Tipo,
                    CategoriaCaminho = parcela.Agendamento.Categoria.ObterCaminho(),
                    Pessoa = parcela.Agendamento.Pessoa?.Nome,
                    QuantidadeParcelas = parcela.Agendamento.ObterQuantidadeParcelas()
                }
                : null;
        }

        public ParcelaSaida(
            int id,
            int? idAgendamento,
            int? idFatura,
            DateTime data,
            decimal valor,
            int numero,
            bool lancada,
            bool descartada,
            string motivoDescarte,
            string observacao,
            DateTime? dataLancamento = null,
            object agendamento = null)
        {
            Id             = id;
            IdAgendamento  = idAgendamento;
            IdFatura       = idFatura;
            Data           = data;
            Valor          = valor;
            Numero         = numero;
            Lancada        = lancada;
            DataLancamento = dataLancamento;
            Descartada     = descartada;
            MotivoDescarte = motivoDescarte;
            Observacao     = observacao;
            Agendamento    = agendamento;
        }

        public override string ToString()
        {
            return $"Parcela {this.Numero} - {this.Data.ToString("dd/MM/yyyy")} - {this.Valor.ToString("C2")}";
        }
    }
}
