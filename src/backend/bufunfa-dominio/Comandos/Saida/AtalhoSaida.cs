using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de um atalho
    /// </summary>
    public class AtalhoSaida
    {
        /// <summary>
        /// ID do atalho
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Título do atalho
        /// </summary>
        public string Titulo { get; }

        /// <summary>
        /// URL do atalho
        /// </summary>
        public string Url { get; }

        public AtalhoSaida(Atalho atalho)
        {
            if (atalho == null)
                return;

            this.Id     = atalho.Id;
            this.Titulo = atalho.Titulo;
            this.Url    = atalho.Url?.ToLower();
        }

        public AtalhoSaida(int id, string titulo, string url)
        {
            Id     = id;
            Titulo = titulo;
            Url    = url;
        }

        public override string ToString()
        {
            return this.Titulo + " - " + this.Url;
        }
    }
}
