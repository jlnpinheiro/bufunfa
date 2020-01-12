using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class LancamentoViewModel
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoMensagem), ErrorMessageResourceName = "Id_Conta_Invalido")]
        public int? IdConta { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoMensagem), ErrorMessageResourceName = "Id_Categoria_Invalido")]
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        [Required(ErrorMessage = "A data do lançamento é obrigatória e não foi informada.")]
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? Data { get; set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        [Required(ErrorMessage = "O valor do lançamento é obrigatório e não foi informado.")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Quantidade de ações (quando renda variável)
        /// </summary>
        public int? QuantidadeAcoes { get; set; }

        /// <summary>
        /// Observação sobre o lançamento
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(LancamentoMensagem), ErrorMessageResourceName = "Observacao_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }
    }
}
