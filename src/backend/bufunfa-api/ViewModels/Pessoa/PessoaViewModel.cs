using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class PessoaViewModel
    {
        /// <summary>
        /// Nome da pessoa
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(PessoaMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(200, ErrorMessageResourceType = typeof(PessoaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }
    }
}
