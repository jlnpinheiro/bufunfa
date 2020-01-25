using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de agendamentos
    /// </summary>
    public class ProcurarAgendamentoEntrada : Notificavel
    {
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; set; }

        public int? IdCategoria { get; set; }

        public int? IdConta { get; set; }

        public int? IdCartaoCredito { get; set; }

        public int? IdPessoa { get; set; }

        public DateTime? DataInicioParcela { get; set; }

        public DateTime? DataFimParcela { get; set; }

        public bool? Concluido { get; set; }

        public ProcurarAgendamentoEntrada(int idUsuario)
        {
            this.IdUsuario = idUsuario;
            
            this.Validar();
        }

        public void Validar()
        {
            if (this.DataInicioParcela.HasValue && this.DataFimParcela.HasValue)
                this.NotificarSeMaiorQue(this.DataInicioParcela.Value, this.DataFimParcela.Value, AgendamentoMensagem.Agendamento_Procurar_Periodo_Invalido);

            if (this.DataInicioParcela.HasValue && !this.DataFimParcela.HasValue || !this.DataInicioParcela.HasValue && this.DataFimParcela.HasValue)
                this.AdicionarNotificacao(AgendamentoMensagem.Agendamento_Procurar_Periodo_Invalido);
        }

        public bool Paginar()
        {
            return this.PaginaIndex.HasValue && this.PaginaTamanho.HasValue;
        }
    }
}
