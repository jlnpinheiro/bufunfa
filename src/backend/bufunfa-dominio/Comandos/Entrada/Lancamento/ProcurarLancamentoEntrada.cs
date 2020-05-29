using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;
using System.ComponentModel;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de lançamentos
    /// </summary>
    public class ProcurarLancamentoEntrada : ProcurarEntrada<LancamentoOrdenarPor>
    {
        public int? IdConta { get; }

        public int? IdCategoria { get; }

        public int? IdPessoa { get; }

        public DateTime? DataInicio { get; }

        public DateTime? DataFim { get; }

        public ProcurarLancamentoEntrada(
            int idUsuario,
            int? idConta = null,
            int? idCategoria = null,
            int? idPessoa = null,
            DateTime? dataInicio = null,
            DateTime? dataFim = null,
            LancamentoOrdenarPor ordenarPor = LancamentoOrdenarPor.Data,
            string ordenarSentido = "ASC",
            int? paginaIndex = 1,
            int? paginaTamanho = 10) 
            : base(idUsuario, ordenarPor, ordenarSentido, paginaIndex, paginaTamanho)
        {
            IdConta     = idConta;
            IdCategoria = idCategoria;
            IdPessoa    = idPessoa;
            DataInicio  = dataInicio;
            DataFim     = dataFim;

            this.Validar();
        }

        private void Validar()
        {
            if (this.DataInicio.HasValue && this.DataFim.HasValue)
                this.NotificarSeMaiorQue(this.DataInicio.Value, this.DataFim.Value, LancamentoMensagem.Lancamento_Procurar_Periodo_Invalido);

            if (this.DataInicio.HasValue && !this.DataFim.HasValue || !this.DataInicio.HasValue && this.DataFim.HasValue)
                this.NotificarSeMaiorQue(this.DataInicio.Value, this.DataFim.Value, LancamentoMensagem.Lancamento_Procurar_Periodo_Invalido);
        }
    }

    public enum LancamentoOrdenarPor
    {
        [Description("Caminho da categoria associada ao lançamento")]
        CategoriaCaminho,
        [Description("Nome da pessoa associada ao lançamento")]
        NomePessoa,
        [Description("Nome da conta associada ao lançamento")]
        NomeConta,
        [Description("Valor do lançamento")]
        Valor,
        [Description("Data do lançamento")]
        Data
    }
}
