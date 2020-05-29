using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;
using System.ComponentModel;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de agendamentos
    /// </summary>
    public class ProcurarAgendamentoEntrada : ProcurarEntrada<AgendamentoOrdenarPor>
    {
        public int? IdCategoria { get; }

        public int? IdConta { get; }

        public int? IdCartaoCredito { get; }

        public int? IdPessoa { get; }

        public DateTime? DataInicioParcela { get; }

        public DateTime? DataFimParcela { get; }

        public bool? Concluido { get; }

        public ProcurarAgendamentoEntrada(
            int idUsuario,
            int? idCategoria = null,
            int? idConta = null,
            int? idCartaoCredito = null,
            int? idPessoa = null,
            DateTime? dataInicioParcela = null,
            DateTime? dataFimParcela = null,
            bool? concluido = null,
            AgendamentoOrdenarPor ordenarPor = AgendamentoOrdenarPor.DataProximaParcela,
            string ordenarSentido = "ASC",
            int? paginaIndex = 1,
            int? paginaTamanho = 10)
            : base(idUsuario, ordenarPor, ordenarSentido, paginaIndex, paginaTamanho)
        {
            IdCategoria       = idCategoria;
            IdConta           = idConta;
            IdCartaoCredito   = idCartaoCredito;
            IdPessoa          = idPessoa;
            DataInicioParcela = dataInicioParcela;
            DataFimParcela    = dataFimParcela;
            Concluido         = concluido;

            this.Validar();
        }

        public void Validar()
        {
            if (this.DataInicioParcela.HasValue && this.DataFimParcela.HasValue)
                this.NotificarSeMaiorQue(this.DataInicioParcela.Value, this.DataFimParcela.Value, AgendamentoMensagem.Agendamento_Procurar_Periodo_Invalido);

            if (this.DataInicioParcela.HasValue && !this.DataFimParcela.HasValue || !this.DataInicioParcela.HasValue && this.DataFimParcela.HasValue)
                this.AdicionarNotificacao(AgendamentoMensagem.Agendamento_Procurar_Periodo_Invalido);
        }
    }

    public enum AgendamentoOrdenarPor
    {
        [Description("Data da próxima parcela do agendamento")]
        DataProximaParcela,
        [Description("Data da última parcela do agendamento")]
        DataUltimaParcela,
        [Description("Caminho da categoria associada ao agendamento")]
        CategoriaCaminho,
        [Description("Nome da pessoa associada ao agendamento")]
        NomePessoa,
        [Description("Nome da conta associada ao agendamento")]
        NomeConta,
        [Description("Nome do cartão de crédito associado ao agendamento")]
        NomeCartaoCredito
    }
}
