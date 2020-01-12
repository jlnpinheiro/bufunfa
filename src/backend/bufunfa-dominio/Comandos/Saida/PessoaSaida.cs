using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de uma pessoa
    /// </summary>
    public class PessoaSaida
    {
        /// <summary>
        /// ID da pessoa
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; }

        public PessoaSaida(Pessoa pessoa)
        {
            if (pessoa == null)
                return;

            this.Id   = pessoa.Id;
            this.Nome = pessoa.Nome;
        }

        public PessoaSaida(int id, string nome)
        {
            Id   = id;
            Nome = nome;
        }

        public override string ToString()
        {
            return this.Nome;
        }
    }
}
