using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando utilizado para o cadastro de uma nova categoria
    /// </summary>
    public class CategoriaEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário proprietário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Id da categoria pai
        /// </summary>
        public int? IdCategoriaPai { get; }
        
        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; }

        /// <summary>
        /// Tipo da categoria
        /// </summary>
        public string Tipo { get; }

        public CategoriaEntrada(
            int idUsuario,
            string nome,
            string tipo,
            int? idCategoriaPai = null)
        {
            this.IdUsuario      = idUsuario;
            this.Nome           = nome;
            this.Tipo           = tipo?.ToUpper();
            this.IdCategoriaPai = idCategoriaPai;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Nome, CategoriaMensagem.Nome_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.Tipo, CategoriaMensagem.Tipo_Obrigatorio_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 100, CategoriaMensagem.Nome_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.Tipo))
                this.NotificarSeVerdadeiro(this.Tipo != "D" && this.Tipo != "C", CategoriaMensagem.Tipo_Invalido);

            if (this.IdCategoriaPai.HasValue)
                this.NotificarSeMenorQue(this.IdCategoriaPai.Value, 1, CategoriaMensagem.Id_Categoria_Pai_Invalido);
        }
    }
}