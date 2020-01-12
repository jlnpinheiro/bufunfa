using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para realizar a transferência de valores entre contas
    public class TransferenciaViewModel
    {
        /// <summary>
        /// Id da conta origem
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Id_Conta_Origem_Transferencia_Invalido")]
        public int? IdContaOrigem { get; set; }

        /// <summary>
        /// Id da conta destino
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoMensagem), ErrorMessageResourceName = "Id_Conta_Destino_Transferencia_Invalido")]
        public int? IdContaDestino { get; set; }

        /// <summary>
        /// Data da transferência
        /// </summary>
        [Required(ErrorMessage = "A data da tranferência é obrigatória e não foi informada.")]
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? Data { get; set; }

        /// <summary>
        /// Valor da transferência
        /// </summary>
        [Required(ErrorMessage = "O valor da transferência é obrigatório e não foi informado.")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Observação da transferência
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Observacao_Transferencia_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }
    }
}
