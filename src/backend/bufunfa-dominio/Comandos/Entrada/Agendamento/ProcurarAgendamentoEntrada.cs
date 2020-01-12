using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de agendamentos
    /// </summary>
    public class ProcurarAgendamentoEntrada : ProcurarEntrada
    {
        public int? IdCategoria { get; set; }

        public int? IdConta { get; set; }

        public int? IdCartaoCredito { get; set; }

        public int? IdPessoa { get; set; }

        public DateTime? DataInicioParcela { get; set; }

        public DateTime? DataFimParcela { get; set; }

        public bool? Concluido { get; set; }

        public ProcurarAgendamentoEntrada(
            int idUsuario,
            string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                idUsuario,
                string.IsNullOrEmpty(ordenarPor) ? "DataProximaParcelaAberta" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
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
}
