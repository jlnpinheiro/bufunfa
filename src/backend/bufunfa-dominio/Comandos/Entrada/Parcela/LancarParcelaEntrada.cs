using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando utilizado para lançar uma parcela
    /// </summary>
    public class LancarParcelaEntrada : Notificavel
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Data do lançamento da parcela
        /// </summary>
        public DateTime Data { get; }

        /// <summary>
        /// Valor do lançamento da parcela
        /// </summary>
        public decimal Valor { get; }

        /// <summary>
        /// Observação sobre o lançamento da parcela
        /// </summary>
        public string Observacao { get; }

        public LancarParcelaEntrada(
            int idUsuario,
            DateTime data,
            decimal valor,
            string observacao = null)
        {
            this.IdUsuario   = idUsuario;
            this.Data        = data;
            this.Valor       = valor;
            this.Observacao  = observacao;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeMaiorQue(this.Data, DateTime.Today, ParcelaMensagem.Data_Lancamento_Maior_Data_Corrente);

            if (!string.IsNullOrEmpty(this.Observacao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Observacao, 500, ParcelaMensagem.Observacao_Tamanho_Maximo_Excedido);
        }
    }
}
