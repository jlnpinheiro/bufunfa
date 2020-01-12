using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class AlterarSenhaUsuarioViewModel
    {
        /// <summary>
        /// Senha do usuário
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(UsuarioMensagem), ErrorMessageResourceName = "Senha_Obrigatoria_Nao_Informada")]
        public string Senha { get; set; }

        /// <summary>
        /// Nova senha do usuário
        /// </summary>
        [Required(ErrorMessage = "A senha nova é obrigatória e não foi informada.")]
        public string SenhaNova { get; set; }

        /// <summary>
        /// Confirmação da nova senha do usuário
        /// </summary>
        [Required(ErrorMessage = "A confirmação da nova senha é obrigatória e não foi informada.")]
        public string ConfirmarSenhaNova { get; set; }
    }
}
