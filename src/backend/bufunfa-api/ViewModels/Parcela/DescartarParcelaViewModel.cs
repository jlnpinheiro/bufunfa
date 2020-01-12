using JNogueira.Bufunfa.Dominio.Resources;
using System.ComponentModel.DataAnnotations;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o descarte de uma parcela
    public class DescartarParcelaViewModel
    {
        /// <summary>
        /// Motivo do descarte da parcela
        /// </summary>
        [MaxLength(500, ErrorMessageResourceType = typeof(ParcelaMensagem), ErrorMessageResourceName = "Motivo_Descarte_Tamanho_Maximo_Excedido")]
        public string MotivoDescarte { get; set; }
    }
}
