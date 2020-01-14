using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando utilizado para entrada referente ao pagamento de uma fatura
    /// </summary>
    public class PagarFaturaEntrada : Notificavel
    {
        /// <summary>
        /// Id do usuário responsável
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID do cartão de crédito
        /// </summary>
        public int IdCartaoCredito { get; }

        /// <summary>
        /// Mês da fatura
        /// </summary>
        public int MesFatura { get; }

        /// <summary>
        /// Ano da fatura
        /// </summary>
        public int AnoFatura { get; }

        /// <summary>
        /// Valor adicional creditado a fatura
        /// </summary>
        public decimal? ValorAdicionalCredito { get; }

        /// <summary>
        /// Observação sobre o valor adicional creditado a fatura
        /// </summary>
        public string ObservacaoCredito { get; }

        /// <summary>
        /// Valor adicional debitado a fatura
        /// </summary>
        public decimal? ValorAdicionalDebito { get; }

        /// <summary>
        /// Observação sobre o valor adicional debitado a fatura
        /// </summary>
        public string ObservacaoDebito { get; }

        /// <summary>
        /// ID da conta onde o lançamento referente ao pagamento da fatura será criado
        /// </summary>
        public int IdContaPagamento { get; }

        /// <summary>
        /// ID da pessoa relacionada ao lançamento referente ao pagamento da fatura será criado
        /// </summary>
        public int? IdPessoaPagamento { get; }

        /// <summary>
        /// Data do pagamento da fatura
        /// </summary>
        public DateTime DataPagamento { get; }

        /// <summary>
        /// Valor do pagamento da fatura
        /// </summary>
        public decimal ValorPagamento { get; }

        public PagarFaturaEntrada(
            int idUsuario,
            int idCartaoCredito,
            int mesFatura,
            int anoFatura,
            int idContaPagamento,
            DateTime dataPagamento,
            decimal valorPagamento,
            int? idPessoaPagamento = null,
            decimal? valorAdicionalCredito = null,
            string observacaoCredito = null,
            decimal? valorAdicionalDebito = null,
            string observacaoDebito = null)
        {
            this.IdUsuario             = idUsuario;
            this.IdCartaoCredito       = idCartaoCredito;
            this.MesFatura             = mesFatura;
            this.AnoFatura             = anoFatura;
            this.IdContaPagamento      = idContaPagamento;
            this.DataPagamento         = dataPagamento;
            this.ValorPagamento        = valorPagamento;
            this.IdPessoaPagamento     = idPessoaPagamento;
            this.ValorAdicionalCredito = valorAdicionalCredito;
            this.ObservacaoCredito     = observacaoCredito;
            this.ValorAdicionalDebito  = valorAdicionalDebito;
            this.ObservacaoDebito      = observacaoDebito;

            Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeMenorOuIgualA(this.IdCartaoCredito, 0, CartaoCreditoMensagem.Id_Cartao_Invalido)
                .NotificarSeMenorOuIgualA(this.IdContaPagamento, 0, ContaMensagem.Id_Conta_Invalido);
        }
    }
}
