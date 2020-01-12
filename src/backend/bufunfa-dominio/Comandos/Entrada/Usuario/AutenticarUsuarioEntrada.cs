using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using NETCore.Encrypt.Extensions;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando utilizado na autenticação de um usuário
    /// </summary>
    public class AutenticarUsuarioEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Senha { get; }

        public AutenticarUsuarioEntrada(string email, string senha)
        {
            this.Email = email;
            this.Senha = senha?.MD5();
        }

        public void Validar()
        {
            this
                .NotificarSeNuloOuVazio(this.Email, UsuarioMensagem.Email_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Senha, UsuarioMensagem.Senha_Obrigatoria_Nao_Informada);

            if (!string.IsNullOrEmpty(this.Email))
                this.NotificarSeEmailInvalido(this.Email, string.Format(UsuarioMensagem.Email_Invalido, this.Email));
        }
    }
}
