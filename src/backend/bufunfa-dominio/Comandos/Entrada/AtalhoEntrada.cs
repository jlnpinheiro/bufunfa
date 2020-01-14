using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public class AtalhoEntrada : Notificavel
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Título do atalho
        /// </summary>
        public string Titulo { get; }

        /// <summary>
        /// URL do atalho
        /// </summary>
        public string Url { get; }

        public AtalhoEntrada(int idUsuario, string titulo, string url)
        {
            this.IdUsuario = idUsuario;
            this.Titulo    = titulo;
            this.Url       = url?.ToLower();

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Titulo, AtalhoMensagem.Titulo_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Url, AtalhoMensagem.Url_Obrigatorio_Nao_Informado);
        }
    }
}
