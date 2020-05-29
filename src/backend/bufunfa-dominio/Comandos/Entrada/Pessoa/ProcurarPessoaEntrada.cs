using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System.ComponentModel;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Classe com opções de filtro para procura de pessoas
    /// </summary>
    public class ProcurarPessoaEntrada : ProcurarEntrada<PessoaOrdenarPor>
    {
        public string Nome { get; }

        public ProcurarPessoaEntrada(
            int idUsuario,
            string nome = null,
            PessoaOrdenarPor ordenarPor = PessoaOrdenarPor.Nome,
            string ordenarSentido = "ASC",
            int? paginaIndex = 1,
            int? paginaTamanho = 10)
            : base(idUsuario, ordenarPor, ordenarSentido, paginaIndex, paginaTamanho)
        {
            this.Nome = nome;
            
            this.Validar();
        }

        private void Validar()
        {
            if (!string.IsNullOrEmpty(this.Nome))
                this.NotificarSePossuirTamanhoSuperiorA(this.Nome, 200, PessoaMensagem.Nome_Tamanho_Maximo_Excedido);
        }
    }

    public enum PessoaOrdenarPor
    {
        [Description("ID da pessoa")]
        Id,
        [Description("Nome da pessoa")]
        Nome
    }
}
