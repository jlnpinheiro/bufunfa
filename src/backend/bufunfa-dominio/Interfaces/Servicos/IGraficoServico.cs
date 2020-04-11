using JNogueira.Bufunfa.Dominio.Interfaces.Comandos;
using System.Threading.Tasks;

namespace JNogueira.Bufunfa.Dominio.Interfaces.Servicos
{
    /// <summary>
    /// Interface que expõe os serviços relacionados a geração de gráficos
    /// </summary>
    public interface IGraficoServico
    {
        /// <summary>
        /// Obtém as informações para geração do gráfico de relação entre valor e categoria por ano
        /// </summary>
        Task<ISaida> GerarGraficoRelacaoValorCategoriaPorAno(int ano, int idCategoria, int idUsuario);

        /// <summary>
        /// Obtém as informações específicas de um período que compõem o gráfico de relação entre valor e categoria por ano
        /// </summary>
        Task<ISaida> ObterPeriodoGraficoRelacaoValorCategoriaPorAno(int idPeriodo, int ano, int idCategoria, int idUsuario);
    }
}
