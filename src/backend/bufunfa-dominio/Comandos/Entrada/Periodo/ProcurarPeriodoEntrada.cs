using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;
using System.ComponentModel;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de períodos
    /// </summary>
    public class ProcurarPeriodoEntrada : ProcurarEntrada<PeriodoOrdenarPor>
    {
        public string Nome { get; }

        public DateTime? Data { get; }

        public ProcurarPeriodoEntrada(
            int idUsuario,
            string nome = null,
            DateTime? data = null,
            PeriodoOrdenarPor ordenarPor = PeriodoOrdenarPor.DataInicio,
            string ordenarSentido = "ASC",
            int? paginaIndex = 1,
            int? paginaTamanho = 10)
            : base(idUsuario, ordenarPor, ordenarSentido, paginaIndex, paginaTamanho)
        {
            Nome = nome;
            Data = data;

            this.Validar();
        }

        private void Validar()
        {
            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 50, PeriodoMensagem.Nome_Tamanho_Maximo_Excedido);

            if (this.Data.HasValue)
                this.NotificarSeMenorQue(this.Data.Value.Date, new DateTime(2015, 1, 1), string.Format(PeriodoMensagem.Periodo_Procura_Data_Invalida, new DateTime(2015, 1, 1).ToString("dd/MM/yyyy")));
        }
    }

    public enum PeriodoOrdenarPor
    {
        [Description("Nome do período")]
        Nome,
        [Description("Data início do período")]
        DataInicio,
        [Description("Data fim do período")]
        DataFim
    }
}
