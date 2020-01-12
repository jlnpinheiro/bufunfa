using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class AutenticarUsuarioViewModel
    {
        /// <summary>
        /// Email do usuário
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(UsuarioMensagem), ErrorMessageResourceName = "Email_Obrigatorio_Nao_Informado")]
        public string Email { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(UsuarioMensagem), ErrorMessageResourceName = "Senha_Obrigatoria_Nao_Informada")]
        public string Senha { get; set; }
    }
}
