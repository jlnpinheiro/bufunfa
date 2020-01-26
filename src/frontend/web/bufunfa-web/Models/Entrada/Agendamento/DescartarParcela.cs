namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada dos dados para o descarte de uma parcela
    /// </summary>
    public class DescartarParcela : BaseModel
    {
        /// <summary>
        /// Id da parcela
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Descrição do motivo do descarta da parcela
        /// </summary>
        public string MotivoDescarte { get; set; }
    }
}
