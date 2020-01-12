using JNogueira.Bufunfa.Dominio.Entidades;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de sáida para as informações de um anexo
    /// </summary>
    public class LancamentoAnexoSaida
    {
        /// <summary>
        /// ID do anexo
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Id do lançamento
        /// </summary>
        public int IdLancamento { get; }

        /// <summary>
        /// Id do arquivo no Google Drive
        /// </summary>
        public string IdGoogleDrive { get; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        public string NomeArquivo { get; }

        public LancamentoAnexoSaida(LancamentoAnexo anexo)
        {
            if (anexo == null)
                return;

            this.Id            = anexo.Id;
            this.IdLancamento  = anexo.IdLancamento;
            this.IdGoogleDrive = anexo.IdGoogleDrive;
            this.Descricao     = anexo.Descricao;
            this.NomeArquivo   = anexo.NomeArquivo;
        }

        public LancamentoAnexoSaida(
            int id,
            int idLancamento,
            string idGoogleDrive,
            string descricao,
            string nomeArquivo)
        {
            Id            = id;
            IdLancamento  = idLancamento;
            IdGoogleDrive = idGoogleDrive;
            Descricao     = descricao;
            NomeArquivo   = nomeArquivo;
        }

        public override string ToString()
        {
            return $"{this.Descricao} - {this.NomeArquivo}";
        }
    }
}
