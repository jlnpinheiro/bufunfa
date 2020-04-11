using JNogueira.Bufunfa.Web.Helpers;
using JNogueira.Bufunfa.Web.Models;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Web.Proxy
{
    public partial class BackendProxy
    {
        /// <summary>
        /// Obtém as informações para geração do gráfico de relação entre valor e categoria por ano
        /// </summary>
        public async Task<Saida<GraficoRelacaoValorCategoriaPorAno>> GerarGraficoRelacaoValorCategoriaPorAno(int ano, int idCategoria) => await _httpClientHelper.FazerRequest<Saida<GraficoRelacaoValorCategoriaPorAno>>("grafico/obter-grafico-relacao-valor-categoria-por-ano?ano=" + ano + "&idCategoria=" + idCategoria, MetodoHttp.GET);

        /// <summary>
        /// Obtém as informações específicas de um período que compõem o gráfico de relação entre valor e categoria por ano
        /// </summary>
        public async Task<Saida<PeriodoGraficoRelacaoValorCategoriaPorAno>> ObterPeriodoGraficoRelacaoValorCategoriaPorAno(int idPeriodo, int ano, int idCategoria) => await _httpClientHelper.FazerRequest<Saida<PeriodoGraficoRelacaoValorCategoriaPorAno>>("grafico/obter-periodo-grafico-relacao-valor-categoria-por-ano?idPeriodo=" + idPeriodo + "&ano=" + ano + "&idCategoria=" + idCategoria, MetodoHttp.GET);
    }
}
