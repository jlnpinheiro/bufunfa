using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class LancamentoDetalheViewModel
    {
        /// <summary>
        /// Id da categoria
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(CategoriaMensagem), ErrorMessageResourceName = "Id_Categoria_Invalido")]
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Valor do detalhe
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoDetalheMensagem), ErrorMessageResourceName = "Id_Categoria_Obrigatorio_Nao_Informado")]
        public decimal? Valor { get; set; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(LancamentoDetalheMensagem), ErrorMessageResourceName = "Observacao_Tamanho_Maximo_Excedido")]
        public string Observacao { get; set; }
    }
}
