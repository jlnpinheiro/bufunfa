using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando utilizado para entrada referente a um transferência entre contas
    /// </summary>
    public class TransferenciaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário responsável
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// ID da conta de origem
        /// </summary>
        public int IdContaOrigem { get; }

        /// <summary>
        /// ID da conta de destino
        /// </summary>
        public int IdContaDestino { get; }

        /// <summary>
        /// Data da transferência
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor da transferência
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observações referentes a transferência
        /// </summary>
        public string Observacao { get; }

        public TransferenciaEntrada(int idUsuario, int idContaOrigem, int idContaDestino, DateTime data, decimal valor, string observacao)
        {
            IdUsuario      = idUsuario;
            IdContaOrigem  = idContaOrigem;
            IdContaDestino = idContaDestino;
            Data           = data;
            Valor          = valor;
            Observacao     = observacao;

            Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeMenorOuIgualA(this.IdContaOrigem, 0, ContaMensagem.Id_Conta_Origem_Transferencia_Invalido)
                .NotificarSeMenorOuIgualA(this.IdContaDestino, 0, ContaMensagem.Id_Conta_Destino_Transferencia_Invalido)
                .NotificarSeMenorOuIgualA(this.Valor, 0, ContaMensagem.Valor_Transferencia_Invalido)
                .NotificarSeMaiorQue(this.Data.Date, DateTime.Now.Date, ContaMensagem.Data_Transferencia_Invalida);

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, ContaMensagem.Observacao_Transferencia_Tamanho_Maximo_Excedido);
        }
    }
}
