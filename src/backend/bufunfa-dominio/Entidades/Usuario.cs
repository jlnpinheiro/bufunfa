namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um usuário
    /// </summary>
    public class Usuario
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Nome do usuário
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        public string Email { get; private set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        public string Senha { get; internal set; }

        /// <summary>
        /// Indica se o usuário está ativo
        /// </summary>
        public bool Ativo { get; private set; }

        private Usuario()
        {
            
        }

        public Usuario(string nome, string email, bool ativo = true)
            : this()
        {
            this.Nome = nome;
            this.Email = email;
            this.Ativo = ativo;
        }

        public void AlterarSenha(string novaSenha)
        {
            this.Senha = novaSenha;
        }

        public override string ToString()
        {
            return this.Nome.ToUpper();
        }
    }
}
