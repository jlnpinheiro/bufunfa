using JNogueira.Bufunfa.Dominio.Entidades;
using System.Collections.Generic;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de uma categoria
    /// </summary>
    public class CategoriaSaida
    {
        /// <summary>
        /// ID da categoria
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get;  }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; }

        /// <summary>
        /// Caminho da categoria
        /// </summary>
        public string Caminho { get; }

        /// <summary>
        /// Categoria pai
        /// </summary>
        public object CategoriaPai { get; }

        /// <summary>
        /// Categorias filha
        /// </summary>
        public IEnumerable<object> CategoriasFilha { get; }
       
        public CategoriaSaida(Categoria categoria)
        {
            if (categoria == null)
                return;

            this.Id     = categoria.Id;
            this.Nome   = categoria.Nome;
            this.Tipo   = categoria.Tipo;
            this.Caminho = categoria.ObterCaminho();

            if (categoria.CategoriaPai != null)
            {
                this.CategoriaPai = new
                {
                    categoria.CategoriaPai.Id,
                    categoria.CategoriaPai.IdCategoriaPai,
                    categoria.CategoriaPai.Nome,
                    categoria.CategoriaPai.Tipo,
                    Caminho = categoria.CategoriaPai.ObterCaminho()
                };
            }

            this.CategoriasFilha = ObterCategoriasFilha(categoria.CategoriasFilha);
        }

        public CategoriaSaida(
            int id,
            string nome,
            string tipo,
            string caminho,
            CategoriaSaida categoriaPai = null,
            IEnumerable<CategoriaSaida> categoriasFilha = null)
        {
            Id              = id;
            Nome            = nome;
            Tipo            = tipo;
            Caminho         = caminho;
            CategoriaPai    = categoriaPai;
            CategoriasFilha = categoriasFilha;
        }

        private IEnumerable<object> ObterCategoriasFilha(IEnumerable<Categoria> categorias)
        {
            if (categorias == null || !categorias.Any())
                return null;

            return categorias
                .OrderBy(x => x.Nome)
                .Select(x => new
                {
                    x.Id,
                    x.Nome,
                    x.Tipo,
                    Caminho = x.ObterCaminho(),
                    CategoriasFilha = ObterCategoriasFilha(x.CategoriasFilha)
                }).ToList();
        }
        
        public override string ToString()
        {
            return $"{this.Nome} ({this.Caminho})";
        }
    }
}
