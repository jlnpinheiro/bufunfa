using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o cadastro e alteração de um agendamento
    public class AgendamentoViewModel
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// Id do cartão de crédito
        /// </summary>
        public int? IdCartaoCredito { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Id_Categoria_Obrigatorio_Nao_Informado")]
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Tipo do método de pagamento das parcelas
        /// </summary>
        [EnumDataType(typeof(MetodoPagamento), ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Tipo_Metodo_Pagamento_Invalido_Ou_Nao_Informado")]
        public MetodoPagamento TipoMetodoPagamento { get; set; }

        /// <summary>
        /// Observação sobre o agendamento
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Observacao_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }

        /// <summary>
        /// Valor de cada parcela do agendamento
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Valor_Parcela_Nao_Informado")]
        public decimal? ValorParcela { get; set; }

        /// <summary>
        /// Data da primeira parcela do agendamento
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Data_Primeira_Parcela_Obrigatoria_Nao_Informada")]
        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? DataPrimeiraParcela { get; set; }

        /// <summary>
        /// Quantidade de parcelas
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Quantidade_Parcelas_Obrigatoria_Nao_Informada")]
        public int? QuantidadeParcelas { get; set; }

        /// <summary>
        /// Periodicidade das parcelas
        /// </summary>
        [EnumDataType(typeof(Periodicidade), ErrorMessageResourceType = typeof(AgendamentoMensagem), ErrorMessageResourceName = "Periodicidade_Parcelas_Obrigatorio_Nao_Informado")]
        public Periodicidade PeriodicidadeParcelas { get; set; }
    }
}
