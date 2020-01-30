using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Bufunfa.Dominio.Interfaces.Dados;
using JNogueira.Bufunfa.Dominio.Resources;
using JNogueira.Bufunfa.Infraestrutura.Integracoes.Google;
using JNogueira.NotifiqueMe;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Infraestrutura.Dados.Repositorios
{
    public class LancamentoAnexoRepositorio : Notificavel, ILancamentoAnexoRepositorio
    {
        private readonly IPeriodoRepositorio _periodoRepositorio;
        private readonly EfDataContext _efContext;
        private readonly ApiGoogleDriveProxy _apiGoogleDriveProxy;

        /// <summary>
        /// ID da pasta raiz no Google Drive onde os anexos serão armazenados ("Bufunfa - Anexos")
        /// </summary>
        private string _idPastaGoogleDrive;

        public LancamentoAnexoRepositorio(EfDataContext efContext, IPeriodoRepositorio periodoRepositorio, ConfigurationHelper configHelper)
        {
            _apiGoogleDriveProxy = new ApiGoogleDriveProxy();
            _efContext           = efContext;
            _periodoRepositorio  = periodoRepositorio;
            _idPastaGoogleDrive  = configHelper.IdPastaGoogleDriveAnexos;
        }

        public async Task<LancamentoAnexo> ObterPorId(int idAnexo) => await _efContext.LancamentosAnexo.Include(x => x.Lancamento).FirstOrDefaultAsync(x => x.Id == idAnexo);

        public async Task<LancamentoAnexo> Inserir(int idLancamento, DateTime dataLancamento, LancamentoAnexoEntrada cadastroEntrada)
        {
            // Realiza o upload do arquivo do anexo para o Google Drive
            var idGoogleDrive = await RealizarUploadAnexo(dataLancamento, cadastroEntrada);

            if (this.Invalido)
                return null;

            var anexo = new LancamentoAnexo(idLancamento, cadastroEntrada, idGoogleDrive);

            await _efContext.AddAsync(anexo);

            return anexo;
        }

        public async Task Deletar(LancamentoAnexo anexo)
        {
            if (_apiGoogleDriveProxy.Invalido)
            {
                this.AdicionarNotificacoes(_apiGoogleDriveProxy.Notificacoes);
                return;
            }

            // Exclui o arquivo do anexo do Google Drive
            await _apiGoogleDriveProxy.ExcluirPorId(anexo.IdGoogleDrive);

            _efContext.LancamentosAnexo.Remove(anexo);
        }

        public async Task<MemoryStream> Download(string idAnexoGoogleDrive)
        {
            return await _apiGoogleDriveProxy.RealizarDownload(idAnexoGoogleDrive);
        }

        /// <summary>
        /// Realiza o upload de um arquivo
        /// </summary>
        private async Task<string> RealizarUploadAnexo(DateTime dataLancamento, LancamentoAnexoEntrada cadastroEntrada)
        {
            if (_apiGoogleDriveProxy.Invalido)
            {
                this.AdicionarNotificacoes(_apiGoogleDriveProxy.Notificacoes);
                return null;
            }

            this.NotificarSeNuloOuVazio(_idPastaGoogleDrive, "O ID da pasta no Google Drive não foi informado.");

            if (this.Invalido)
                return null;

            var periodo = await _periodoRepositorio.ObterPorData(dataLancamento, cadastroEntrada.IdUsuario);

            Google.Apis.Drive.v3.Data.File pastaPeriodo;

            if (periodo != null)
            {
                // Caso exista um período, é criado uma pasta com o nome do período
                pastaPeriodo = await _apiGoogleDriveProxy.CriarPasta(periodo.Nome, _idPastaGoogleDrive);
            }
            else
            {
                // Caso não exista um período, é criado uma pasta com a partir do mês/ano do lançamento
                pastaPeriodo = await _apiGoogleDriveProxy.CriarPasta(dataLancamento.ToString("MM/yyyy"), _idPastaGoogleDrive);
            }

            // Verifica se um arquivo com o mesmo nome já existe na pasta do mês do lançamento
            var anexoJaExistente = await _apiGoogleDriveProxy.ProcurarPorNome(ApiGoogleDriveProxy.TipoGoogleDriveFile.Arquivo, cadastroEntrada.NomeArquivo, pastaPeriodo.Id);

            this.NotificarSeNaoNulo(anexoJaExistente, LancamentoAnexoMensagem.Nome_Arquivo_Ja_Existe_Google_Drive);

            if (this.Invalido)
                return null;

            // Realiza o upload do arquivo
            return await _apiGoogleDriveProxy.RealizarUpload(cadastroEntrada.NomeArquivo, cadastroEntrada.MimeTypeArquivo, cadastroEntrada.ConteudoArquivo, cadastroEntrada.Descricao, pastaPeriodo.Id);
        }
    }
}