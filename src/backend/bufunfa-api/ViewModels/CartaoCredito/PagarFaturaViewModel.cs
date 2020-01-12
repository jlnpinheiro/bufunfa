using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class PagarFaturaViewModel
    {
        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(CartaoCreditoMensagem), ErrorMessageResourceName = "Id_Cartao_Invalido")]
        public int? IdCartaoCredito { get; set; }

        /// <summary>
        /// Mês da fatura
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(CartaoCreditoMensagem), ErrorMessageResourceName = "Fatura_Mes_Obrigatorio_Nao_Informado")]
        public int? MesFatura { get; set; }

        /// <summary>
        /// Ano da fatura
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(CartaoCreditoMensagem), ErrorMessageResourceName = "Fatura_Ano_Obrigatorio_Nao_Informado")]
        public int? AnoFatura { get; set; }

        /// <summary>
        ///  Valor adicional creditado a fatura
        /// </summary>
        public decimal? ValorAdicionalCredito { get; set; }

        /// <summary>
        /// Observação sobre o valor adicional creditado a fatura
        /// </summary>
        [MaxLength(100, ErrorMessageResourceType = typeof(CartaoCreditoMensagem), ErrorMessageResourceName = "Observacao_Credito_Tamanho_Maximo_Excedido")]
        public string ObservacaoCredito { get; set; }

        /// <summary>
        ///  Valor adicional debitado a fatura
        /// </summary>
        public decimal? ValorAdicionalDebito { get; set; }

        /// <summary>
        /// Observação sobre o valor adicional debitado a fatura
        /// </summary>
        [MaxLength(100, ErrorMessageResourceType = typeof(CartaoCreditoMensagem), ErrorMessageResourceName = "Observacao_Debito_Tamanho_Maximo_Excedido")]
        public string ObservacaoDebito { get; set; }

        /// <summary>
        /// Ano da fatura
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Id_Conta_Nao_Informado")]
        public int? IdContaPagamento { get; set; }

        /// <summary>
        /// ID da pessoa relacionada ao lançamento referente ao pagamento da fatura será criado
        /// </summary>
        public int? IdPessoaPagamento { get; set; }

        /// <summary>
        /// Data do pagamento da fatura
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(CartaoCreditoMensagem), ErrorMessageResourceName = "Fatura_Data_Pagamento_Obrigatorio_Nao_Informado")]
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataPagamento { get; set; }

        /// <summary>
        /// Valor do pagamento da fatura
        /// </summary>
        [Required(ErrorMessage = "O valor do pagamento da fatura é obrigatório e não foi informado.")]
        public decimal? ValorPagamento { get; set; }
    }
}
