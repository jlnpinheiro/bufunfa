using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class PessoaSaidaExemplo : PessoaSaida
    {
        public PessoaSaidaExemplo()
            : base (1, "Supermercado Carone")
        {

        }
    }

    public class PessoaRequestExemplo : IExamplesProvider<PessoaViewModel>
    {
        public PessoaViewModel GetExamples()
        {
            return new PessoaViewModel
            {
                Nome = "Supermecado Carone"
            };
        }
    }

    public class CadastrarPessoaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Cadastrada_Com_Sucesso },
                Retorno = new PessoaSaidaExemplo()
            };
        }
    }

    public class AlterarPessoaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Alterada_Com_Sucesso },
                Retorno = new PessoaSaidaExemplo()
            };
        }
    }

    public class ExcluirPessoaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Excluida_Com_Sucesso },
                Retorno = new PessoaSaidaExemplo()
            };
        }
    }

    public class ObterPessoaPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PessoaMensagem.Pessoa_Encontrada_Com_Sucesso },
                Retorno = new PessoaSaidaExemplo()
            };
        }
    }

    public class ProcurarPessoaRequestExemplo : IExamplesProvider<ProcurarPessoaViewModel>
    {
        public ProcurarPessoaViewModel GetExamples()
        {
            return new ProcurarPessoaViewModel
            {
                Nome = "Fulano",
                OrdenarPor = "Nome",
                OrdenarSentido = "ASC",
                PaginaIndex = 1,
                PaginaTamanho = 50
            };
        }
    }

    public class ProcurarPessoaResponseExemplo : IExamplesProvider<ProcurarSaida>
    {
        public ProcurarSaida GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new PessoaSaidaExemplo()
                    }, "Nome", "ASC", 3, 1, 1, 3)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }
}
