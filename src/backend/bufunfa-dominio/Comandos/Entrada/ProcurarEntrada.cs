using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public abstract class ProcurarEntrada<TOrdenarPor> : Notificavel
    {
        public int IdUsuario { get; }

        /// <summary>
        /// Página atual da listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaIndex { get; }

        /// <summary>
        /// Quantidade de registros exibidos por página na listagem que exibirá o resultado da pesquisa
        /// </summary>
        public int? PaginaTamanho { get; }

        /// <summary>
        /// Tipo de ordenação que deverá ser utilizada no resultado da pesquisa
        /// </summary>
        public TOrdenarPor OrdenarPor { get; }

        /// <summary>
        /// Sentido da ordenação do resultado da pesquisa: "ASC" para crescente / "DESC" para decrescente
        /// </summary>
        public string OrdenarSentido { get; }

        public ProcurarEntrada(int idUsuario, TOrdenarPor ordenarPor, string ordenarSentido = "ASC", int? paginaIndex = 1, int? paginaTamanho = 10)
        {
            this.IdUsuario = idUsuario;
            this.OrdenarPor = ordenarPor;
            this.OrdenarSentido = !string.Equals(ordenarSentido, "ASC", StringComparison.CurrentCultureIgnoreCase) && !string.Equals(ordenarSentido, "DESC", StringComparison.InvariantCultureIgnoreCase)
                ? "ASC"
                : ordenarSentido.ToUpper();
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