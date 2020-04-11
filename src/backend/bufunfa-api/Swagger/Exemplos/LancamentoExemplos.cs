using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class LancamentoRequestExemplo : IExamplesProvider<LancamentoViewModel>
    {
        public LancamentoViewModel GetExamples()
        {
            return new LancamentoViewModel
            {
                Data        = DateTime.Now,
                IdCategoria = 4,
                IdConta     = 3,
                IdPessoa    = 30,
                Observacao  = "Observação do lançamento",
                Valor       = (decimal)23.34
            };
        }
    }

    public class LancamentoSaidaExemplo : LancamentoSaida
    {
        public LancamentoSaidaExemplo()
            : base(
                1,
                DateTime.Now,
                (decimal)23.34,
                new ContaSaida(3, "Conta X", TipoConta.ContaCorrente, (decimal)115.54, "Banco Santander S/A", "3345", "01005539-0"),
                new CategoriaSaida(4, "Salário", TipoCategoria.Credito, "CRÉDITO » Salário"),
                new PessoaSaida(1, "Meu patrão"),
                new ParcelaSaida(2, 2, null, DateTime.Now, (decimal)12.12, 1, false, false, null, null),
                new LancamentoAnexoSaida(1, 1, "1gF8wE6OVfCnghANI70A-gh9rXc-jNGob", "Comprovante", "comprovante.pdf"),
                new LancamentoDetalheSaida(1, 1, 100, new CategoriaSaida(4, "Salário", TipoCategoria.Credito, "CRÉDITO » Salário"), new LancamentoSaidaExemplo(), "Observação do detalhe"),
                "Observação qualquer")
        {

        }
    }

    public class CadastrarLancamentoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Cadastrado_Com_Sucesso },
                Retorno = new LancamentoSaidaExemplo()
            };
        }
    }

    public class AlterarLancamentoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Alterado_Com_Sucesso },
                Retorno = new LancamentoSaidaExemplo()
            };
        }
    }

    public class ExcluirLancamentoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Excluido_Com_Sucesso },
                Retorno = new LancamentoSaidaExemplo()
            };
        }
    }

    public class ObterLancamentoPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { LancamentoMensagem.Lancamento_Encontrado_Com_Sucesso },
                Retorno = new LancamentoSaidaExemplo()
            };
        }
    }

    public class ProcurarLancamentoRequestExemplo : IExamplesProvider<ProcurarLancamentoViewModel>
    {
        public ProcurarLancamentoViewModel GetExamples()
        {
            return new ProcurarLancamentoViewModel
            {
                DataInicio = DateTime.Now.Date,
                DataFim = DateTime.Now.AddDays(5).Date,
                IdCategoria = 80,
                IdConta = 1,
                IdPessoa = null,
                OrdenarPor = "Data",
                OrdenarSentido = "DESC",
                PaginaIndex = 1,
                PaginaTamanho = 50
            };
        }
    }

    public class ProcurarLancamentoResponseExemplo : IExamplesProvider<ProcurarSaida>
    {
        public ProcurarSaida GetExamples()
        {
            return new ProcurarSaida(new[] {
                        new LancamentoSaidaExemplo()
                    }, "Nome", "ASC", 3, 1, 1, 3)
            {
                Mensagens = new[] { Mensagem.Procura_Resultado_Com_Sucesso }
            };
        }
    }
}
