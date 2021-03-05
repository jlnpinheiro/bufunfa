using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Servicos
{
    public class UsuarioServico : Notificavel
    {
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IUnitOfWork _uow;

        public UsuarioServico(IUsuarioRepositorio usuarioRepositorio, IUnitOfWork uow)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _uow                = uow;
        }

        public async Task<ISaida> Autenticar(AutenticarUsuarioEntrada entrada)
        {
            // Verifica se o e-mail e a senha do usuário foi informado.
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorEmailSenha(entrada.Email, entrada.Senha);

            // Verifica se o usuário com o e-mail e a senha (hash) foi encontrado no banco
            this.NotificarSeNulo(usuario, UsuarioMensagem.Usuario_Nao_Encontrado_Por_Login_Senha);

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            // Verifica se o usuário está ativo
            this.NotificarSeFalso(usuario.Ativo, UsuarioMensagem.Usuario_Inativo);

            return this.Invalido
                ? new Saida(false, this.Mensagens, null)
                : new Saida(true, new[] { UsuarioMensagem.Usuario_Autenticado_Com_Sucesso }, new UsuarioSaida(usuario));
        }

        public async Task<ISaida> AlterarSenha(AlterarSenhaUsuarioEntrada entrada)
        {
            if (entrada.Invalido)
                return new Saida(false, entrada.Mensagens, null);

            var usuario = await _usuarioRepositorio.ObterPorEmailSenha(entrada.Email, entrada.SenhaAtual, true);

            // Verifica se o usuário com o e-mail e a senha (hash) foi encontrado no banco
            this.NotificarSeNulo(usuario, "Usuário não encontrado. Verifique a senha informada.");

            if (this.Invalido)
                return new Saida(false, this.Mensagens, null);

            usuario.AlterarSenha(entrada.SenhaNova);

            await _uow.Commit();

            return new Saida(true, new[] { "Senha de acesso alterada com sucesso." }, null);
        }
    }
}
