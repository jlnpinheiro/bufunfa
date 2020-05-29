using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de categorias
    /// </summary>
    public class ProcurarCategoriaEntrada : Notificavel
    {
        public int IdUsuario { get; }

        public string Nome { get; }

        public int? IdCategoriaPai { get; }

        public string Tipo { get; }

        public string Caminho { get; }

        public ProcurarCategoriaEntrada(int idUsuario, string nome = null, int? idCategoriaPai = null, string tipo = null, string caminho = null)
        {
            IdUsuario      = idUsuario;
            Nome           = nome;
            IdCategoriaPai = idCategoriaPai;
            Tipo           = tipo;
            Caminho        = caminho;

            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 100, CategoriaMensagem.Nome_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.Tipo))
                this.NotificarSeVerdadeiro(this.Tipo != "D" && this.Tipo != "C", CategoriaMensagem.Tipo_Invalido);

            if (this.IdCategoriaPai.HasValue)
                this.NotificarSeMenorQue(this.IdCategoriaPai.Value, 1, CategoriaMensagem.Id_Categoria_Pai_Invalido);
        }
    }
}
