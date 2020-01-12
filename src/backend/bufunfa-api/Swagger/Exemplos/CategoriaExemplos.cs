using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class ProcurarCategoriaRequestExemplo : IExamplesProvider<ProcurarCategoriaViewModel>
    {
        public ProcurarCategoriaViewModel GetExamples()
        {
            return new ProcurarCategoriaViewModel
            {
                Caminho = "DÉBITO » Alimentação » Restaurante",
                IdCategoriaPai = null,
                Nome = "Restaurante",
                Tipo = TipoCategoria.Debito
            };
        }
    }

    public class CategoriaSaidaExemplo : CategoriaSaida
    {
        public CategoriaSaidaExemplo()
            : base(1, "Alimentação", TipoCategoria.Debito, "DÉBITO » Alimentação", new CategoriaSaida(3, "Restaurante ou lanchonetes", TipoCategoria.Debito, "DÉBITO » Alimentação » Restaurante ou lanchonetes"))
        {

        }
    }

    public class ProcurarCategoriaResponseExemplo : IExamplesProvider<ProcurarSaida>
    {
        public ProcurarSaida GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new CategoriaSaidaExemplo()
                    }, "Nome", "ASC", 1, 1, 1, 1)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }

    public class CategoriaRequestExemplo : IExamplesProvider<CategoriaViewModel>
    {
        public CategoriaViewModel GetExamples()
        {
            return new CategoriaViewModel
            {
                Nome = "Restaurante",
                Tipo = TipoCategoria.Debito,
                IdCategoriaPai = 1
            };
        }
    }

    public class CadastrarCategoriaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Cadastrada_Com_Sucesso },
                Retorno = new CategoriaSaidaExemplo()
            };
        }
    }

    public class AlterarCategoriaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Alterada_Com_Sucesso },
                Retorno = new CategoriaSaidaExemplo()
            };
        }
    }

    public class ExcluirCategoriaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Excluida_Com_Sucesso },
                Retorno = new CategoriaSaidaExemplo()
            };
        }
    }

    public class ObterCategoriaPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categoria_Encontrada_Com_Sucesso },
                Retorno = new CategoriaSaidaExemplo()
            };
        }
    }

    public class ObterCategoriasPorUsuarioResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CategoriaMensagem.Categorias_Encontradas_Com_Sucesso },
                Retorno = new[]
                {
                    new CategoriaSaidaExemplo()
                }
            };
        }
    }
}
