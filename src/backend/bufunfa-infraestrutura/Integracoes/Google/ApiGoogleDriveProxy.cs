using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoogleApiV3Data = Google.Apis.Drive.v3.Data;

namespace JNogueira.Bufunfa.Infraestrutura.Integracoes.Google
{
    /// <summary>
    /// Classe que permite a integração com o Google Drive
    /// </summary>
    public class ApiGoogleDriveProxy : Notificavel
    {
        public enum TipoGoogleDriveFile
        {
            Arquivo,
            Pasta
        }

        private readonly DriveService _driveService;

        public ApiGoogleDriveProxy()
        {
            try
            {
                var googleCredentialsFilePath = Path.Combine(Directory.GetCurrentDirectory(), "google_credentials.json");

                this.NotificarSeFalso(File.Exists(googleCredentialsFilePath), "O arquivo \"google_credentials.json\" contendo as credenciais de acesso para utilização do Google Service Account não foi encontrado.");

                if (this.Invalido)
                    return;

                string[] scopes = new string[] { DriveService.Scope.Drive };

                GoogleCredential credential;

                using (var stream = new FileStream(googleCredentialsFilePath, FileMode.Open, FileAccess.Read))
                {
                    credential = GoogleCredential
                                        .FromStream(stream)
                                        .CreateScoped(scopes);
                }

                _driveService = new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential
                });
            }
            catch (Exception ex)
            {
                this.AdicionarNotificacao("Não é possível realizar a integração com a API do Google Drive: " + ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// Realiza a procura de um item pelo nome
        /// </summary>
        /// <param name="tipo">Tipo do item (pasta ou arquivo).</param>
        /// <param name="nome">Nome do item que deverá ser procurado.</param>
        /// <param name="idPastaProcura">ID da pasta onde a procura deverá ser realizada.</param>
        public async Task<GoogleApiV3Data.File> ProcurarPorNome(TipoGoogleDriveFile tipo, string nome, string idPastaProcura = null)
        {
            FilesResource.ListRequest list = _driveService.Files.List();
            list.Fields = "files(id, name, trashed, parents)";
            list.PageSize = 5;
            list.Q = $"name = '{nome}' and trashed = false";

            if (tipo == TipoGoogleDriveFile.Pasta)
                list.Q += " and mimeType = 'application/vnd.google-apps.folder'";

            if (!string.IsNullOrEmpty(idPastaProcura))
                list.Q += " and '" + idPastaProcura + "' in parents";

            var filesFeed = await list.ExecuteAsync();

            while (filesFeed.Files != null)
            {
                var encontrado = filesFeed.Files.FirstOrDefault(x => string.Equals(x.Name, nome, StringComparison.InvariantCultureIgnoreCase));

                if (encontrado != null)
                    return encontrado;

                if (filesFeed.NextPageToken == null)
                    break;

                list.PageToken = filesFeed.NextPageToken;

                filesFeed = await list.ExecuteAsync();
            }

            return null;
        }

        /// <summary>
        /// Cria uma nova pasta
        /// </summary>
        /// <param name="nome">Nome da pasta</param>
        /// <param name="idPastaPai">ID da pasta onde a pasta deverá ser criada.</param>
        public async Task<GoogleApiV3Data.File> CriarPasta(string nome, string idPastaPai = null)
        {
            var pasta = await ProcurarPorNome(TipoGoogleDriveFile.Pasta, nome, idPastaPai);

            if (pasta != null)
                return pasta;

            var fileMetadata = new GoogleApiV3Data.File()
            {
                Name = nome,
                MimeType = "application/vnd.google-apps.folder",
                Parents = !string.IsNullOrEmpty(idPastaPai) ? new List<string>() { idPastaPai } : null
            };

            var request = _driveService.Files.Create(fileMetadata);
            request.Fields = "id";
            return await request.ExecuteAsync();
        }

        /// <summary>
        /// Realiza o upload de um arquivo
        /// </summary>
        /// <param name="nomeArquivo">Nome do arquivo com a extensão.</param>
        /// <param name="mimeType">Mime type do arquivo</param>
        /// <param name="conteudoArquivo">Conteúdo do arquivo qu será feito upload.</param>
        /// <param name="descricaoArquivo">Descrição do arquivo.</param>
        /// <param name="idPastaPai">ID da pasta onde o arquivo será armazenado.</param>
        public async Task<string> RealizarUpload(string nomeArquivo, string mimeType, byte[] conteudoArquivo, string descricaoArquivo = null, string idPastaPai = null)
        {
            var fileMetadata = new GoogleApiV3Data.File
            {
                Name = nomeArquivo,
                Description = descricaoArquivo,
                MimeType = mimeType,
                Parents = !string.IsNullOrEmpty(idPastaPai) ? new List<string>() { idPastaPai } : null
            };

            var stream = new MemoryStream(conteudoArquivo);

            FilesResource.CreateMediaUpload request = _driveService.Files.Create(fileMetadata, stream, mimeType);
            await request.UploadAsync();
            return request.ResponseBody.Id;
        }

        /// <summary>
        /// Realiza o download de um item a partir do seu ID.
        /// </summary>
        public async Task<MemoryStream> RealizarDownload(string id)
        {
            var request = _driveService.Files.Get(id);
            var stream = new MemoryStream();

            await request.DownloadAsync(stream);

            return stream;
        }

        /// <summary>
        /// Realiza a exclusão de um item a partir do seu nome.
        /// </summary>
        /// <param name="tipo">Tipo do item (pasta ou arquivo).</param>
        /// <param name="nome">Nome do arquivo.</param>
        /// <param name="idPastaPai">ID da pasta onde o arquivo está armazenado.</param>
        public async Task ExcluirPorNome(TipoGoogleDriveFile tipo, string nome, string idPastaPai = null)
        {
            var file = await ProcurarPorNome(tipo, nome, idPastaPai);

            if (file == null)
                return;

            FilesResource.DeleteRequest deleteRequest = _driveService.Files.Delete(file.Id);
            await deleteRequest.ExecuteAsync();
        }

        /// <summary>
        /// Realiza a exclusão de um item a partir do seu ID.
        /// </summary>
        /// <param name="id">ID do arquivo que será excluído.</param>
        public async Task ExcluirPorId(string id)
        {
            FilesResource.DeleteRequest deleteRequest = _driveService.Files.Delete(id);
            await deleteRequest.ExecuteAsync();
        }
    }
}
