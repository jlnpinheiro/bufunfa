using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class PeriodoViewModel
    {
        /// <summary>
        /// Nome do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(50, ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// Data início do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Data_Inicio_Obrigatoria_Nao_Informada")]
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Data fim do período
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Data_Fim_Obrigatoria_Nao_Informada")]
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataFim { get; set; }
    }
}
