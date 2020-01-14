using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public abstract class ProcurarEntrada : Notificavel
    {
        public int IdUsuario { get; private set; }

        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; private set; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; private set; }

        /// <summary>
        /// Nome da propriedade que deverá ser utilizada para ordenação do resultado da pesquisa
        /// </summary>
        public string OrdenarPor { get; private set; }

        /// <summary>
        /// Sentido da ordenação do resultado da pesquisa: "ASC" para crescente / "DESC" para decrescente
        /// </summary>
        public string OrdenarSentido { get; private set; }

        public ProcurarEntrada(int idUsuario, string ordenarPor, string ordenarSentido, int? paginaIndex = null, int? paginaTamanho = null)
        {
            this.IdUsuario = idUsuario;
            this.OrdenarPor = ordenarPor;
            this.OrdenarSentido = !string.Equals(ordenarSentido, "ASC", StringComparison.InvariantCultureIgnoreCase) && !string.Equals(ordenarSentido, "DESC", StringComparison.InvariantCultureIgnoreCase)
                ? "ASC"
                : ordenarSentido;
            this.PaginaIndex = paginaIndex;
            this.PaginaTamanho = paginaTamanho;

            this.Validar();
        }

        public bool Paginar()
        {
            return this.PaginaIndex.HasValue && this.PaginaTamanho.HasValue;
        }

        private void Validar()
        {
            this.NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido);

            if (this.PaginaIndex.HasValue)
                this.NotificarSeMenorQue(this.PaginaIndex.Value, 1, Mensagem.Paginacao_Pagina_Index_Invalido);

            if (this.PaginaTamanho.HasValue)
                this.NotificarSeMenorQue(this.PaginaTamanho.Value, 1, Mensagem.Paginacao_Pagina_Tamanho_Invalido);
        }
    }
}