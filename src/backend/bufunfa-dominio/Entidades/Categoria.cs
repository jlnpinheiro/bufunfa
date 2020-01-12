using JNogueira.Bufunfa.Dominio.Comandos;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa uma categoria
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// ID da categoria
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int? IdUsuario { get; private set; }

        /// <summary>
        /// ID da categoria pai
        /// </summary>
        public int? IdCategoriaPai { get; private set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; private set; }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; private set; }

        /// <summary>
        /// Categoria pai
        /// </summary>
        public Categoria CategoriaPai { get; private set; }

        /// <summary>
        /// Categorias filha
        /// </summary>
        public IEnumerable<Categoria> CategoriasFilha { get; private set; }

        private Categoria()
        {
            this.CategoriasFilha = new List<Categoria>();
        }

        public Categoria(CategoriaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.IdUsuario      = entrada.IdUsuario;
            this.IdCategoriaPai = entrada.IdCategoriaPai;
            this.Nome           = entrada.Nome;
            this.Tipo           = entrada.Tipo;
        }

        public void Alterar(CategoriaEntrada entrada)
        {
            if (entrada.Invalido)
                return;

            this.Nome           = entrada.Nome;
            this.IdCategoriaPai = entrada.IdCategoriaPai;
            this.Tipo           = entrada.Tipo;
        }

        public override string ToString()
        {
            return $"{this.Nome} ({this.ObterCaminho()})";
        }

        /// <summary>
        /// Indica se a categoria é pai de pelo menos uma outra categoria.
        /// </summary>
        public bool VerificarSePai() => this.CategoriasFilha != null && this.CategoriasFilha.Any();

        /// <summary>
        /// Obtém a descrição do tipo da categoria
        /// </summary>
        public string ObterDescricaoTipo()
        {
            switch (this.Tipo)
            {
                case "C": return "Crédito";
                case "D": return "Débito";
                default: return string.Empty;
            }
        }

        /// <summary>
        /// Obtém o caminho da categoria
        /// </summary>
        public string ObterCaminho()
        {
            return this.CategoriaPai != null
                    ? ObterNomeCategoriaPai(this)
                    : $"{this.ObterDescricaoTipo().ToUpper()} » " + this.Nome;
        }

        private static string ObterNomeCategoriaPai(Categoria categoria)
        {
            if (categoria.IdCategoriaPai.HasValue && categoria.CategoriaPai != null)
                return $"{ObterNomeCategoriaPai(categoria.CategoriaPai)} » {categoria.Nome}";

            return $"{categoria.ObterDescricaoTipo().ToUpper()} » " + categoria.Nome;
        }
    }
}