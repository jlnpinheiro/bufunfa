using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class AgendamentoRequestExemplo : IExamplesProvider<AgendamentoViewModel>
    {
        public AgendamentoViewModel GetExamples()
        {
            return new AgendamentoViewModel
            {
                DataPrimeiraParcela   = DateTime.Now.Date,
                IdCartaoCredito       = null,
                IdCategoria           = 1,
                IdConta               = 1,
                IdPessoa              = null,
                Observacao            = "TV a cabo",
                PeriodicidadeParcelas = Periodicidade.Mensal,
                QuantidadeParcelas    = 12,
                ValorParcela          = (decimal?)100.23
            };
        }
    }

    public class AgendamentoSaidaExemplo : AgendamentoSaida
    {
        public AgendamentoSaidaExemplo()
            : base(
                1,  
                MetodoPagamento.Debito,
                "Observação do agendamento",
                new ContaSaida(3, "Conta X", TipoConta.ContaCorrente, (decimal)115.54, "Banco Santander S/A", "3345", "01005539-0"),
                null,
                new PessoaSaida(1, "Pessoa X"),
                new CategoriaSaida(4, "Salário", TipoCategoria.Credito, "CRÉDITO » Salário"),
                new List<ParcelaSaida> { new ParcelaSaida(1, 1, null, DateTime.Now, (decimal)11.34, 1, false, false, null, "Parcela 1")},
                DateTime.Now.AddMonths(1),
                (decimal?)100.20,
                DateTime.Now.AddMonths(2),
                2,
                2,
                0,
                false,
                1000,
                80)
        {

        }
    }

    public class CadastrarAgendamentoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Cadastrado_Com_Sucesso },
                Retorno = new AgendamentoSaidaExemplo()
            };
        }
    }

    public class AlterarAgendamentoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Alterado_Com_Sucesso },
                Retorno = new AgendamentoSaidaExemplo()
            };
        }
    }

    public class ExcluirAgendamentoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Excluido_Com_Sucesso },
                Retorno = new AgendamentoSaidaExemplo()
            };
        }
    }

    public class ObterAgendamentoPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { AgendamentoMensagem.Agendamento_Encontrado_Com_Sucesso },
                Retorno = new AgendamentoSaidaExemplo()
            };
        }
    }

    public class ObterParcelasPorPeriodoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcelas_Encontradas_Com_Sucesso },
                Retorno = new[] { new ParcelaSaidaExemplo() }
            };
        }
    }

    public class ProcurarAgendamentoRequestExemplo : IExamplesProvider<ProcurarAgendamentoViewModel>
    {
        public ProcurarAgendamentoViewModel GetExamples()
        {
            return new ProcurarAgendamentoViewModel
            {
                Concluido = false,
                DataInicioParcela = DateTime.Today,
                DataFimParcela = DateTime.Today.AddDays(5),
                IdCartaoCredito = null,
                IdCategoria = 80,
                IdConta = 1,
                IdPessoa = null,
                PaginaIndex = 1,
                PaginaTamanho = 10
            };
        }
    }

    public class ProcurarAgendamentoResponseExemplo : IExamplesProvider<ProcurarSaida>
    {
        public ProcurarSaida GetExamples()
        {
            return new ProcurarSaida(new[]
                    {
                        new AgendamentoSaidaExemplo()
                    }, "Nome", "ASC", 3, 1, 1, 3)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }
}
