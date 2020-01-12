using JNogueira.Bufunfa.Dominio.Comandos;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um atalho
    /// </summary>
    public class Atalho
    {
        /// <summary>
        /// ID do atalho
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Título do atalho
        /// </summary>
        public string Titulo { get; private set; }

        /// <summary>
        /// URL do atalho
        /// </summary>
        public string Url { get; private set; }

        private Atalho()
        {
        }

        public Atalho(AtalhoEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdUsuario = entrada.IdUsuario;
            this.Titulo    = entrada.Titulo;
            this.Url       = entrada.Url;
        }

        public void Alterar(AtalhoEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Titulo = entrada.Titulo;
            this.Url    = entrada.Url;
        }

        public override string ToString()
        {
            return this.Titulo + " - " + this.Url;
        }
    }
}