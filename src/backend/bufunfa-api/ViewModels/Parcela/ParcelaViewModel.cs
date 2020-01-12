using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o cadastro e alteração de uma parcela
    public class ParcelaViewModel
    {
        /// <summary>
        /// Data da parcela
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Data_Obrigatoria_Nao_Informada")]
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? Data { get; set; }

        /// <summary>
        /// Valor da parcela
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Valor_Obrigatorio_Nao_Informado")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Observação sobre a parcela
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Observacao_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }
    }
}
