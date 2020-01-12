using JNogueira.Bufunfa.Dominio.Comandos;

namespace JNogueira.Bufunfa.Dominio.Entidades
{
    /// <summary>
    /// Classe que representa um anexo
    /// </summary>
    public class LancamentoAnexo
    {
        /// <summary>
        /// ID do anexo
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; private set; }

        /// <summary>
        /// Id do arquivo no Google Drive
        /// </summary>
        public string IdGoogleDrive { get; private set; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; private set; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        public string NomeArquivo { get; private set; }

        /// <summary>
        /// Lançamento do anexo
        /// </summary>
        public Lancamento Lancamento { get; private set; }

        private LancamentoAnexo()
        {
        }

        public LancamentoAnexo(int idLancamento, LancamentoAnexoEntrada entrada, string idGoogleDrive)
        {
            if (entrada.Invalido)
                return;

            this.IdLancamento  = idLancamento;
            this.IdGoogleDrive = idGoogleDrive;
            this.Descricao     = entrada.Descricao;
            this.NomeArquivo   = entrada.NomeArquivo;
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.NomeArquivo}";
        }
    }
}