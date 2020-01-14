using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando utilizado para descartar uma parcela
    /// </summary>
    public class DescartarParcelaEntrada : Notificavel
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Descrição do motivo do descarta da parcela
        /// </summary>
        public string MotivoDescarte { get; }

        public DescartarParcelaEntrada(
            int idUsuario,
            string motivoDescarte = null)
        {
            this.IdUsuario      = idUsuario;
            this.MotivoDescarte = motivoDescarte;

            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido);


            if (!string.IsNullOrEmpty(this.MotivoDescarte))
                this.NotificarSePossuirTamanhoSuperiorA(this.MotivoDescarte, 500, ParcelaMensagem.Motivo_Descarte_Tamanho_Maximo_Excedido);
        }
    }
}
