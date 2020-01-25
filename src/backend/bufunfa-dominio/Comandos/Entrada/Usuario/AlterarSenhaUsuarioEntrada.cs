using JNogueira.NotifiqueMe;
using NETCore.Encrypt.Extensions;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public class AlterarSenhaUsuarioEntrada : Notificavel
    {
        public string Email { get; }
        
        public string SenhaAtual { get; }

        public string SenhaNova { get; }

        public string ConfirmacaoSenhaNova { get; }

        public AlterarSenhaUsuarioEntrada(string email, string senhaAtual, string senhaNova, string confirmacaoSenhaNova)
        {
            this.Email                = email;
            this.SenhaAtual           = senhaAtual?.MD5();
            this.SenhaNova            = senhaNova?.MD5();
            this.ConfirmacaoSenhaNova = confirmacaoSenhaNova?.MD5();

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeNuloOuVazio(Email, "O e-mail é obrigatório e não foi informado.")
                .NotificarSeNuloOuVazio(SenhaAtual, "A senha atual é obrigatória e não foi informada.")
                .NotificarSeNuloOuVazio(SenhaNova, "A senha nova é obrigatória e não foi informada.")
                .NotificarSeNuloOuVazio(ConfirmacaoSenhaNova, "A confirmação da nova senha é obrigatória e não foi informada.");

            if (!string.IsNullOrEmpty(this.Email))
                this.NotificarSeEmailInvalido(this.Email, $"O e-mail informado {this.Email} é inválido.");
                    
            if (!string.IsNullOrEmpty(this.SenhaNova) && !string.IsNullOrEmpty(this.ConfirmacaoSenhaNova))
                this.NotificarSeDiferentes(this.SenhaNova, this.ConfirmacaoSenhaNova, "A senha e a confirmação da senha são diferentes. Verifique as senhas informadas.");
        }
    }
}
