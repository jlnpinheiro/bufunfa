using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public class PeriodoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Nome da conta
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Data inicial do período
        /// </summary>
        public DateTime DataInicio { get; }

        /// <summary>
        /// Data final do período
        /// </summary>
        public DateTime DataFim { get; }

        public PeriodoEntrada(
            int idUsuario,
            string nome,
            DateTime dataInicio,
            DateTime dataFim)
        {
            this.IdUsuario  = idUsuario;
            this.Nome       = nome?.ToUpper();
            this.DataInicio = dataInicio.Date;
            this.DataFim    = dataFim.Date.AddHours(23).AddMinutes(59).AddSeconds(59);

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, PeriodoMensagem.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeMaiorOuIgualA(this.DataInicio, this.DataFim, PeriodoMensagem.Data_Periodo_Invalidas);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 50, PeriodoMensagem.Nome_Tamanho_Maximo_Excedido);
        }
    }
}
