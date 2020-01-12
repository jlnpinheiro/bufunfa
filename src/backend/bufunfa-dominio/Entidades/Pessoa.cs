using JNogueira.Bufunfa.Dominio.Comandos;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma pessoa
    /// </summary>
    public class Pessoa
    {
        /// <summary>
        /// ID da pessoa
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Nome da período
        /// </summary>
        public string Nome { get; private set; }

        private Pessoa()
        {
        }

        public Pessoa(PessoaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdUsuario = entrada.IdUsuario;
            this.Nome      = entrada.Nome;
        }

        public void Alterar(PessoaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Nome = entrada.Nome;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}