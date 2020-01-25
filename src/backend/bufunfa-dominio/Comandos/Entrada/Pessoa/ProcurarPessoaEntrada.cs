using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System.Reflection;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de pessoas
    /// </summary>
    public class ProcurarPessoaEntrada : ProcurarEntrada
    {
        public string Nome { get; set; }

        public ProcurarPessoaEntrada(
            int idUsuario,
            string ordenarPor,
            string ordenarSentido,
            int? paginaIndex = null,
            int? paginaTamanho = null)
            : base(
                idUsuario,
                string.IsNullOrEmpty(ordenarPor) ? "Nome" : ordenarPor,
                string.IsNullOrEmpty(ordenarSentido) ? "ASC" : ordenarSentido,
                paginaIndex,
                paginaTamanho)
        {
            this.Validar();
        }

        private void Validar()
        {
            this.NotificarSeNulo(typeof(Pessoa).GetProperty(this.OrdenarPor, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance), string.Format(Mensagem.Paginacao_OrdernarPor_Propriedade_Nao_Existe, this.OrdenarPor));

            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 200, PessoaMensagem.Nome_Tamanho_Maximo_Excedido);
        }
    }
}
