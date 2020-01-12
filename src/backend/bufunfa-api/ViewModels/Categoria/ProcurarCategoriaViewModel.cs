using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar categorias
    public class ProcurarCategoriaViewModel
    {
        /// <summary>
        /// Nome da categoria (palavra-chave)
        /// </summary>
        [MaxLength(100, ErrorMessageResourceType = typeof(CategoriaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// ID da categoria pai da categoria
        /// </summary>
        public int? IdCategoriaPai { get; set; }

        /// <summary>
        /// Tipo da categoria ("D" para débito / "C" para crédito)
        /// </summary>
        [MaxLength(1, ErrorMessage = "O tipo da categoria é inválido.")]
        public string Tipo { get; set; }

        /// <summary>
        /// Caminho da categoria (palavra-chave)
        /// </summary>
        public string Caminho { get; set; }
    }
}
