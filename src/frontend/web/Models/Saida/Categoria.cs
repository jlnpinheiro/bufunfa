using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de saída para os dados de uma categoria
    /// </summary>
    public class Categoria
    {
        /// <summary>
        /// ID da categora
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nome da categoria
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Tipo da categoria (C = crédito, D = débito)
        /// </summary>
        public string Tipo { get; set; }

        /// <summary>
        /// Caminho da categoria
        /// </summary>
        public string Caminho { get; set; }

        /// <summary>
        /// Pai da categoria
        /// </summary>
        public Categoria CategoriaPai { get; set; }

        /// <summary>
        /// Coleção de categorias-filha
        /// </summary>
        public IEnumerable<Categoria> CategoriasFilha { get; set; }

        /// <summary>
        /// Obtém o tipo da categoria
        /// </summary>
        public TipoCategoria ObterTipo() => this.Tipo.Equals("C", StringComparison.CurrentCultureIgnoreCase) ? TipoCategoria.Credito : TipoCategoria.Debito;
    }

    public enum TipoCategoria
    {
        Credito,
        Debito
    }
}
