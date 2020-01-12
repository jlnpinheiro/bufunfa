﻿using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Entidades;
using JNogueira.Infraestrutura.NotifiqueMe;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Dados
{
    /// <summary>
    /// Define o contrato do repositório de anexos
    /// </summary>
    public interface ILancamentoAnexoRepositorio : INotificavel
    {
        /// <summary>
        /// Obtém um anexo a partir do seu ID
        /// </summary>
        /// <param name="habilitarTracking">Indica que o tracking do EF deverá estar habilitado, permitindo alteração dos dados.</param>
        Task<LancamentoAnexo> ObterPorId(int idAnexo, bool habilitarTracking = false);

        /// <summary>
        /// Insere um novo anexo no banco de dados.
        /// </summary>
        Task<LancamentoAnexo> Inserir(int idLancamento, DateTime dataLancamento, LancamentoAnexoEntrada cadastroEntrada);

        /// <summary>
        /// Deleta um anexo
        /// </summary>
        Task Deletar(LancamentoAnexo anexo);

        /// <summary>
        /// Realiza o download do arquivo do anexo.
        /// </summary>
        Task<MemoryStream> Download(string idAnexoGoogleDrive);
    }
}
