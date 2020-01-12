using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class LancamentoDetalheSaidaExemplo : LancamentoDetalheSaida
    {
        public LancamentoDetalheSaidaExemplo()
            : base(1, 1, (decimal)115.32, new CategoriaSaida(4, "Internet", TipoCategoria.Debito, "DÉBITO » Internet"), "Observação do detalhe")
        {

        }
    }

    public class LancamentoDetalheRequestExemplo : IExamplesProvider<LancamentoDetalheViewModel>
    {
        public LancamentoDetalheViewModel GetExamples()
        {
            return new LancamentoDetalheViewModel
            {
                IdCategoria = 3,
                Valor = 100,
                Observacao = "Observação do detalhe"
            };
        }
    }

    public class CadastrarDetalheResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoDetalheMensagem.Detalhe_Cadastrado_Com_Sucesso },
                Retorno = new LancamentoDetalheSaidaExemplo()
            };
        }
    }    

    public class ExcluirDetalheResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoDetalheMensagem.Detalhe_Excluido_Com_Sucesso },
                Retorno = new LancamentoDetalheSaidaExemplo()
            };
        }
    }
}
