using System;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classe de entrada para os dados de um lançamento
    /// </summary>
    public class ManterLancamento : BaseModel
    {
        /// <summary>
        /// Id da lançamento
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Id da conta
        /// </summary>
        public int IdConta { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string NomePessoa { get; set; }

        /// <summary>
        /// Data do lançamento
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// Quantidade de ações
        /// </summary>
        public int? QuantidadeAcoes { get; set; }

        /// <summary>
        /// Observações
        /// </summary>
        public string Observacao { get; set; }
    }
}
