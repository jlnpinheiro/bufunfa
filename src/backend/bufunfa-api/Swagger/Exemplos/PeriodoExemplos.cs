using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class PeriodoSaidaExemplo : PeriodoSaida
    {
        public PeriodoSaidaExemplo()
            : base(1, "Janeiro / 2019", new DateTime(2019, 1, 1), new DateTime(2019, 1, 30))
        {

        }
    }

    public class ProcurarPeriodoRequestExemplo : IExamplesProvider<ProcurarPeriodoViewModel>
    {
        public ProcurarPeriodoViewModel GetExamples()
        {
            return new ProcurarPeriodoViewModel
            {
                Data = DateTime.Now.Date,
                Nome = "JANEIRO",
                OrdenarPor = PeriodoOrdenarPor.Nome,
                OrdenarSentido = "ASC",
                PaginaIndex = 1,
                PaginaTamanho = 10
            };
        }
    }

    public class ProcurarPeriodoResponseExemplo : IExamplesProvider<ProcurarSaida>
    {
        public ProcurarSaida GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new PeriodoSaidaExemplo()
                    }, "DataInicio", "ASC", 1, 1, 1, 1)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }

    public class CadastrarPeriodoRequestExemplo : IExamplesProvider<PeriodoViewModel>
    {
        public PeriodoViewModel GetExamples()
        {
            return new PeriodoViewModel
            {
                Nome = "Junho 2018",
                DataInicio = new DateTime(2018, 6, 1),
                DataFim = new DateTime(2018, 6, 30)
            };
        }
    }

    public class CadastrarPeriodoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Cadastrado_Com_Sucesso },
                Retorno = new
                {
                    Id = 1,
                    Nome = "Junho 2018",
                    DataInicio = new DateTime(2018, 6, 1),
                    DataFim = new DateTime(2018, 6, 30)
                }
            };
        }
    }

    public class PeriodoRequestExemplo : IExamplesProvider<PeriodoViewModel>
    {
        public PeriodoViewModel GetExamples()
        {
            return new PeriodoViewModel
            {
                Nome = "Junho 2018",
                DataInicio = new DateTime(2018, 6, 1),
                DataFim = new DateTime(2018, 6, 30)
            };
        }
    }

    public class AlterarPeriodoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Alterado_Com_Sucesso },
                Retorno = new PeriodoSaidaExemplo()
            };
        }
    }

    public class ExcluirPeridoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Excluido_Com_Sucesso },
                Retorno = new PeriodoSaidaExemplo()
            };
        }
    }

    public class ObterPeriodoPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodo_Encontrado_Com_Sucesso },
                Retorno = new PeriodoSaidaExemplo()
            };
        }
    }

    public class ObterPeriodosPorUsuarioResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { PeriodoMensagem.Periodos_Encontrados_Com_Sucesso },
                Retorno = new[]
                {
                    new PeriodoSaidaExemplo()
                }
            };
        }
    }
}
