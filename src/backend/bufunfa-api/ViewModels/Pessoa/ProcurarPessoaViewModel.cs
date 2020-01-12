using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar a
    public class ProcurarPessoaViewModel : ProcurarViewModel
    {
        /// <summary>
        /// Nome da pessoa
        /// </summary>
        [MaxLength(200, ErrorMessageResourceType = typeof(PessoaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        public ProcurarPessoaViewModel()
        {
            this.OrdenarPor = "Nome";
        }
    }
}
