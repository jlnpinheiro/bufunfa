using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class AtalhoSaidaExemplo : AtalhoSaida
    {
        public AtalhoSaidaExemplo()
            : base (1, "Banco Inter", "https://www.bancointer.com.br")
        {

        }
    }

    public class AtalhoRequestExemplo : IExamplesProvider<AtalhoViewModel>
    {
        public AtalhoViewModel GetExamples()
        {
            return new AtalhoViewModel
            {
                Titulo = "Banco Inter",
                Url = "https://www.bancointer.com.br"
            };
        }
    }

    public class CadastrarAtalhoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AtalhoMensagem.Atalho_Cadastrado_Com_Sucesso },
                Retorno = new AtalhoSaidaExemplo()
            };
        }
    }

    public class AlterarAtalhoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AtalhoMensagem.Atalho_Alterado_Com_Sucesso },
                Retorno = new AtalhoSaidaExemplo()
            };
        }
    }

    public class ExcluirAtalhoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AtalhoMensagem.Atalho_Excluido_Com_Sucesso },
                Retorno = new AtalhoSaidaExemplo()
            };
        }
    }

    public class ObterAtalhoPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AtalhoMensagem.Atalho_Encontrado_Com_Sucesso },
                Retorno = new AtalhoSaidaExemplo()
            };
        }
    }

    public class ObterAtalhosPorUsuarioResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AtalhoMensagem.Atalhos_Encontrados_Com_Sucesso },
                Retorno = new[]
                {
                    new AtalhoSaidaExemplo()
                }
            };
        }
    }
}
