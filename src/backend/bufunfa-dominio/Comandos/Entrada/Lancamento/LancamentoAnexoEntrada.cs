using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    public class LancamentoAnexoEntrada : Notificavel, IEntrada
    {
        /// <summary>
        /// Id do usuário
        /// </summary>
        public int IdUsuario { get; }

        /// <summary>
        /// Descrição do anexo
        /// </summary>
        public string Descricao { get; }

        /// <summary>
        /// Nome do arquivo do anexo
        /// </summary>
        public string NomeArquivo { get; }

        /// <summary>
        /// Conteúdo do arquivo do anexo
        /// </summary>
        public byte[] ConteudoArquivo { get; }

        /// <summary>
        /// Mime type do arquivo (necessário para realizar o upload para o Google Drive)
        /// </summary>
        public string MimeTypeArquivo { get; }

        public LancamentoAnexoEntrada(int idUsuario, string descricao, string nomeArquivo, byte[] conteudoArquivo, string mimeTypeArquivo)
        {
            this.IdUsuario       = idUsuario;
            this.Descricao       = descricao;
            this.NomeArquivo     = nomeArquivo;
            this.ConteudoArquivo = conteudoArquivo;
            this.MimeTypeArquivo = mimeTypeArquivo;

            this.Validar();
        }

        private void Validar()
        {
            this
                .NotificarSeMenorOuIgualA(this.IdUsuario, 0, Mensagem.Id_Usuario_Invalido)
                .NotificarSeNuloOuVazio(this.Descricao, LancamentoAnexoMensagem.Descricao_Obrigatorio_Nao_Informado)
                .NotificarSeNuloOuVazio(this.NomeArquivo, LancamentoAnexoMensagem.Nome_Arquivo_Obrigatorio_Nao_Informado)
                .NotificarSeIguais(this.ConteudoArquivo.Length, 0, LancamentoAnexoMensagem.Arquivo_Conteudo_Nao_Informado);

            if (!string.IsNullOrEmpty(this.Descricao))
                this.NotificarSePossuirTamanhoSuperiorA(this.Descricao, 200, LancamentoAnexoMensagem.Descricao_Tamanho_Maximo_Excedido);

            if (!string.IsNullOrEmpty(this.NomeArquivo))
                this.NotificarSePossuirTamanhoSuperiorA(this.NomeArquivo, 50, LancamentoAnexoMensagem.Nome_Arquivo_Tamanho_Maximo_Excedido);

            if (this.ConteudoArquivo != null)
                this.NotificarSeVerdadeiro((decimal)(this.ConteudoArquivo.Length / 1024) > (5 * 1024), string.Format(LancamentoAnexoMensagem.Arquivo_Tamanho_Nao_Permitido, Math.Round((decimal)(this.ConteudoArquivo.Length / 1024) / 1024, 1)));
        }
    }
}
