using JNogueira.Bufunfa.Dominio.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar períodos
    public class ProcurarPeriodoViewModel : ProcurarViewModel
    {
        /// <summary>
        /// Nome da período
        /// </summary>
        [MaxLength(50, ErrorMessageResourceType = typeof(PeriodoMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// Data abrangida pela data início e fim do périodo (deve ser superior á 01/01/2015).
        /// </summary>
        public DateTime? Data { get; set; }
    }
}
