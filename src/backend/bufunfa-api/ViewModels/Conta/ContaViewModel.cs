using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o cadastro e alteração de uma conta
    public class ContaViewModel
    {
        /// <summary>
        /// Nome da conta
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Nome_Obrigatorio_Nao_Informado")]
        [MaxLength(100, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Nome_Tamanho_Maximo_Excedido")]
        public string Nome { get; set; }

        /// <summary>
        /// Tipo da conta (1 = conta-corrente, 2 = investimento)
        /// </summary>
        [EnumDataType(typeof(TipoConta), ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Tipo_Invalido_Ou_Nao_Informado")]
        public TipoConta Tipo { get; set; }

        /// <summary>
        /// Valor inicial do saldo da conta
        /// </summary>
        public decimal? ValorSaldoInicial { get; set; }

        /// <summary>
        /// Nome da instituição financeira a qual a conta pertence
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Nome_Instituicao_Tamanho_Maximo_Excedido")]
        public string NomeInstituicao { get; set; }

        /// <summary>
        /// Número da agência da conta
        /// </summary>
        [MaxLength(20, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Numero_Agencia_Tamanho_Maximo_Excedido")]
        public string NumeroAgencia { get; set; }

        /// <summary>
        /// Número da conta
        /// </summary>
        [MaxLength(20, ErrorMessageResourceType = typeof(ContaMensagem), ErrorMessageResourceName = "Numero_Tamanho_Maximo_Excedido")]
        public string Numero { get; set; }
    }
}
