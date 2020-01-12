using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class AtalhoViewModel
    {
        /// <summary>
        /// Título do atalho
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AtalhoMensagem), ErrorMessageResourceName = "Titulo_Obrigatorio_Nao_Informado")]
        [MaxLength(50, ErrorMessageResourceType = typeof(AtalhoMensagem), ErrorMessageResourceName = "Titulo_Tamanho_Maximo_Excedido")]
        public string Titulo { get; set; }

        /// <summary>
        /// URL do atalho
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(AtalhoMensagem), ErrorMessageResourceName = "Url_Obrigatorio_Nao_Informado")]
        [MaxLength(3000, ErrorMessageResourceType = typeof(AtalhoMensagem), ErrorMessageResourceName = "Url_Tamanho_Maximo_Excedido")]
        public string Url { get; set; }
    }
}
