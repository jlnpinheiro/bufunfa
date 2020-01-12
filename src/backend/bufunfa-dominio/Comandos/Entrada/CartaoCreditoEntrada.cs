using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public class CartaoCreditoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Nome para identificação do cartão
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Valor do limite do cartão
        /// </summary>
        public decimal ValorLimite { get; }

        /// <summary>
        /// Dia do vencimento da fatura do cartão
        /// </summary>
        public int DiaVencimentoFatura { get; }

        public CartaoCreditoEntrada(
            int idUsuario,
            string nome,
            decimal valorLimite,
            int diaVencimentoFatura)
        {
            this.IdUsuario           = idUsuario;
            this.Nome                = nome;
            this.ValorLimite         = valorLimite;
            this.DiaVencimentoFatura = diaVencimentoFatura;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, CartaoCreditoMensagem.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeMenorOuIgualA(this.ValorLimite, 0, CartaoCreditoMensagem.Valor_Limite_Invalido)
                .NotificarSeFalso(this.DiaVencimentoFatura >= 1 && this.DiaVencimentoFatura <= 31, CartaoCreditoMensagem.Dia_Vencimento_Fatura_Invalido);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 100, CartaoCreditoMensagem.Nome_Tamanho_Maximo_Excedido);
        }
    }
}
