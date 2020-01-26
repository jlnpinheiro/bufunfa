using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Proxy
{
    public partial class BackendProxy
    {
        /// <summary>
        /// Obtém um agendamento a partir do seu ID
        /// </summary>
        public async Task<Saida<Agendamento>> ObterAgendamentoPorId(int id) => await _httpClientHelper.FazerRequest<Saida<Agendamento>>("agendamento/obter-por-id?idAgendamento=" + id, MetodoHttp.GET);

        /// <summary>
        /// Realiza a procura por agendamentos
        /// </summary>
        public async Task<Saida<ResultadoProcura<Agendamento>>> ProcurarAgendamentos(ProcurarAgendamento entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<ResultadoProcura<Agendamento>>>("agendamento/procurar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Cadastra um novo agendamento
        /// </summary>
        public async Task<Saida<Agendamento>> CadastrarAgendamento(ManterAgendamento entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Agendamento>>("agendamento/cadastrar", MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera um agendamento
        /// </summary>
        public async Task<Saida<Agendamento>> AlterarAgendamento(ManterAgendamento entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Agendamento>>("agendamento/alterar?idAgendamento=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Obtém as parcelas pertencentes a um determinado intervalo de tempo
        /// </summary>
        public async Task<Saida<IEnumerable<Parcela>>> ObterParcelasPorPeriodo(DateTime dataInicio, DateTime dataFim, bool somenteParcelasAbertas = true) => await _httpClientHelper.FazerRequest<Saida<IEnumerable<Parcela>>>($"agendamento/obter-parcelas-por-periodo?dataInicio={dataInicio.ToString("dd-MM-yyyy")}&dataFim={dataFim.ToString("dd-MM-yyyy")}&somenteParcelasAbertas={somenteParcelasAbertas.ToString().ToLower()}", MetodoHttp.GET);

        /// <summary>
        /// Exclui um agendamento
        /// </summary>
        public async Task<Saida<Agendamento>> ExcluirAgendamento(int id) => await _httpClientHelper.FazerRequest<Saida<Agendamento>>("agendamento/excluir?idAgendamento=" + id, MetodoHttp.DELETE);

        /// <summary>
        /// Cadastra uma nova parcela para o agendamento
        /// </summary>
        public async Task<Saida<Parcela>> CadastrarParcela(ManterParcela entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Parcela>>("agendamento/cadastrar-parcela?idAgendamento=" + entrada.IdAgendamento, MetodoHttp.POST, content);
            }
        }

        /// <summary>
        /// Altera uma parcela
        /// </summary>
        public async Task<Saida<Parcela>> AlterarParcela(ManterParcela entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Parcela>>("agendamento/alterar-parcela?idParcela=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Realiza o lançamento da parcela
        /// </summary>
        public async Task<Saida<Parcela>> LancarParcela(ManterParcela entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Parcela>>("agendamento/lancar-parcela?idParcela=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Descarta uma parcela
        /// </summary>
        public async Task<Saida<Parcela>> DescartarParcela(DescartarParcela entrada)
        {
            using (var content = new StringContent(entrada.ObterJson(), Encoding.UTF8, "application/json"))
            {
                return await _httpClientHelper.FazerRequest<Saida<Parcela>>("agendamento/descartar-parcela?idParcela=" + entrada.Id, MetodoHttp.PUT, content);
            }
        }

        /// <summary>
        /// Exclui uma parcela
        /// </summary>
        public async Task<Saida<Parcela>> ExcluirParcela(int idParcela) => await _httpClientHelper.FazerRequest<Saida<Parcela>>("agendamento/excluir-parcela?idParcela=" + idParcela, MetodoHttp.DELETE);
    }
}
