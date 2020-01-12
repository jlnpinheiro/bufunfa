using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando utilizado para entrada de uma parcela
    /// </summary>
    public class ParcelaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Data da parcela
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor de parcela
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação da parcela
        /// </summary>
        public string Observacao { get; }

        public ParcelaEntrada(
            int idUsuario,
            DateTime data,
            decimal valor,
            string observacao = null) : this(data, valor, observacao)
        {
            this.IdUsuario = idUsuario;

            this.Validar();
        }

        internal ParcelaEntrada(
            DateTime data,
            decimal valor,
            string observacao = null)
        {
            this.Data = data;
            this.Valor = valor;
            this.Observacao = observacao;
        }

        private void Validar()
        {
            this.NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, ParcelaMensagem.Observacao_Tamanho_Maximo_Excedido);
        }
    }
}
