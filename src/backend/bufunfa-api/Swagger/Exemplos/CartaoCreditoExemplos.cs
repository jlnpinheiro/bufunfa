using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class CartaoCreditoRequestExemplo : IExamplesProvider<CartaoCreditoViewModel>
    {
        public CartaoCreditoViewModel GetExamples()
        {
            return new CartaoCreditoViewModel
            {
                Nome = "Cartão VISA 1",
                ValorLimite = (decimal)5000.00,
                DiaVencimentoFatura = 5
            };
        }
    }

    public class CartaoCreditoSaidaExemplo : CartaoCreditoSaida
    {
        public CartaoCreditoSaidaExemplo()
            : base(1, "Cartão de crédito 1", 5000, 5, (decimal)4354.23)
        {

        }
    }

    public class CadastrarCartaoCreditoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartao_Cadastrado_Com_Sucesso },
                Retorno = new CartaoCreditoSaidaExemplo()
            };
        }
    }

    public class AlterarCartaoCreditoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartao_Alterado_Com_Sucesso },
                Retorno = new CartaoCreditoSaidaExemplo()
            };
        }
    }

    public class ExcluirCartaoCreditoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartao_Excluido_Com_Sucesso },
                Retorno = new CartaoCreditoSaidaExemplo()
            };
        }
    }

    public class ObterCartaoCreditoPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso },
                Retorno = new CartaoCreditoSaidaExemplo()
            };
        }
    }

    public class ObterCartaoCreditosPorUsuarioResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Cartoes_Encontrados_Com_Sucesso },
                Retorno = new[]
                {
                    new CartaoCreditoSaidaExemplo()
                }
            };
        }
    }

    public class FaturaSaidaExemplo : FaturaSaida
    {
        public FaturaSaidaExemplo()
            : base(new CartaoCreditoSaidaExemplo(), new [] { new ParcelaSaidaExemplo() }, DateTime.Now.Month, DateTime.Now.Year, new LancamentoSaidaExemplo())
        {

        }
    }

    public class ObterFaturaPorCartaoCreditoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Fatura_Encontrada_Com_Sucesso },
                Retorno = new FaturaSaidaExemplo()
            };
        }
    }

    public class PagarFaturaRequestExemplo : IExamplesProvider<PagarFaturaViewModel>
    {
        public PagarFaturaViewModel GetExamples()
        {
            return new PagarFaturaViewModel
            {
                IdCartaoCredito = 1,
                IdContaPagamento = 2,
                IdPessoaPagamento = 10,
                MesFatura = 12,
                AnoFatura = 2019,
                DataPagamento = DateTime.Now,
                ValorAdicionalCredito = (decimal)100.25,
                ObservacaoCredito = "Pagamento antecipado",
                ValorPagamento = (decimal)2189.23
            };
        }
    }

    public class PagarFaturaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { CartaoCreditoMensagem.Fatura_Paga_Com_Sucesso },
                Retorno = new FaturaSaidaExemplo()
            };
        }
    }
}
