using JNogueira.Bufunfa.Dominio.Resources;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    public class LancamentoAnexoViewModel
    {
        /// <summary>
        /// Descrição do anexo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoAnexoMensagem), ErrorMessageResourceName = "Descricao_Obrigatorio_Nao_Informado")]
        [MaxLength(200, ErrorMessageResourceType = typeof(LancamentoAnexoMensagem), ErrorMessageResourceName = "Descricao_Tamanho_Maximo_Excedido")]
        public string Descricao { get; set; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoAnexoMensagem), ErrorMessageResourceName = "Nome_Arquivo_Obrigatorio_Nao_Informado")]
        [MaxLength(50, ErrorMessageResourceType = typeof(LancamentoAnexoMensagem), ErrorMessageResourceName = "Nome_Arquivo_Tamanho_Maximo_Excedido")]
        public string NomeArquivo { get; set; }

        /// <summary>
        /// Conteúdo do arquivo do anexo
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(LancamentoAnexoMensagem), ErrorMessageResourceName = "Arquivo_Conteudo_Nao_Informado")]
        public IFormFile Arquivo { get; set; }
    }
}
