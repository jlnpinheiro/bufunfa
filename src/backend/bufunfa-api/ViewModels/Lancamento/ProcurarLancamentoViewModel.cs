using System;

namespace JNogueira.Bufunfa.Api.ViewModels
{
    // View model utilizado para o procurar lançamentos
    public class ProcurarLancamentoViewModel : ProcurarViewModel
    {
        /// <summary>
        /// Id da conta
        /// </summary>
        public int? IdConta { get; set; }

        /// <summary>
        /// Id da categoria
        /// </summary>
        public int? IdCategoria { get; set; }

        /// <summary>
        /// Id da pessoa
        /// </summary>
        public int? IdPessoa { get; set; }

        /// <summary>
        /// Data da início do lançamento
        /// </summary>
        public DateTime? DataInicio { get; set; }

        /// <summary>
        /// Data do fim do lançamento
        /// </summary>
        public DateTime? DataFim { get; set; }


        public ProcurarLancamentoViewModel()
        {
            this.OrdenarPor = "Data";
        }
    }
}
