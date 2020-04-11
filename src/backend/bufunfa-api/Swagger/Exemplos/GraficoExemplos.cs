using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class PeriodoGraficoRelacaoValorCategoriaPorAnoSaidaExemplo : PeriodoGraficoRelacaoValorCategoriaPorAnoSaida
    {
        public PeriodoGraficoRelacaoValorCategoriaPorAnoSaidaExemplo()
            : base(
                new PeriodoSaidaExemplo(),
                new[] { new LancamentoSaidaExemplo() },
                new[] { new LancamentoDetalheSaidaExemplo() },
                new[] { new ParcelaSaidaExemplo() })
        {

        }
    }


    public class GraficoRelacaoValorCategoriaPorAnoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { "Informações obtidas com sucesso." },
                Retorno = new GraficoRelacaoValorCategoriaPorAnoSaida(1, 2020, new CategoriaSaida(12, "Alimentação", TipoCategoria.Debito, "DÉBITO > Alimentação"), new[] { new PeriodoGraficoRelacaoValorCategoriaPorAnoSaidaExemplo() })
            };
        }
    }

    public class ObterPeriodoRelacaoValorCategoriaPorAnoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { "Informações do período obtidas com sucesso." },
                Retorno = new PeriodoGraficoRelacaoValorCategoriaPorAnoSaidaExemplo()
            };
        }
    }
}
