using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class ContaRequestExemplo : IExamplesProvider<ContaViewModel>
    {
        public ContaViewModel GetExamples()
        {
            return new ContaViewModel
            {
                Nome = "Santander - Conta corrente",
                Tipo = TipoConta.ContaCorrente,
                ValorSaldoInicial = (decimal?)115.54,
                NomeInstituicao = "Banco Santander S/A",
                NumeroAgencia = "3345",
                Numero = "01005539-0"
            };
        }
    }

    public class TransferenciaRequestExemplo : IExamplesProvider<TransferenciaViewModel>
    {
        public TransferenciaViewModel GetExamples()
        {
            return new TransferenciaViewModel
            {
                IdContaOrigem = 1,
                IdContaDestino = 2,
                Data = DateTime.Now,
                Valor = (decimal)23.34,
                Observacao = "Observação sobre a transferência."
            };
        }
    }

    public class ContaSaidaExemplo : ContaSaida
    {
        public ContaSaidaExemplo()
            : base(3, "Santander - Conta corrente", TipoConta.ContaCorrente, (decimal)115.54, "Banco Santander S/A", "3345", "01005539-0", (decimal)145.34)
        {

        }
    }

    public class RendaVariavelAnaliseSaidaExemplo : RendaVariavelAnaliseSaida
    {
        public RendaVariavelAnaliseSaidaExemplo()
            : base(
                "BIDI4",
                "Banco Inter",
                TipoConta.Acoes,
                5,
                20,
                650,
                15,
                625,
                20,
                30,
                35,
                new RendaVariavelCotacaoSaida((decimal)15.75, (decimal)16.32, DateTime.Now),
                new[]
                {
                    new RendaVariavelOperacaoSaida(1, DateTime.Now, new { IdTipo = (int)TipoCategoriaEspecial.CompraAcoes, CodigoTipo = TipoCategoria.Credito, DescricaoTipo = TipoCategoriaEspecial.CompraAcoes.ObterDescricao() }, 20, 650, null),
                    new RendaVariavelOperacaoSaida(2, DateTime.Now.AddDays(2), new { IdTipo = (int)TipoCategoriaEspecial.VendaAcoes, CodigoTipo = TipoCategoria.Debito, DescricaoTipo = TipoCategoriaEspecial.VendaAcoes.ObterDescricao() }, 10, 400, null)
                })
        {

        }
    }

    public class CadastrarContaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Cadastrada_Com_Sucesso },
                Retorno = new ContaSaidaExemplo()
            };
        }
    }

    public class AlterarContaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Alterada_Com_Sucesso },
                Retorno = new ContaSaidaExemplo()
            };
        }
    }

    public class ExcluirContaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Excluida_Com_Sucesso },
                Retorno = new ContaSaidaExemplo()
            };
        }
    }

    public class ObterContaPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Conta_Encontrada_Com_Sucesso },
                Retorno = new ContaSaidaExemplo()
            };
        }
    }

    public class ObterContasPorUsuarioResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Contas_Encontradas_Com_Sucesso },
                Retorno = new[]
                {
                    new ContaSaidaExemplo()
                }
            };
        }
    }

    public class ObterAnaliseAcaoResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Analise_Carteira_Obtida_Com_Sucesso },
                Retorno = new[]
                {
                    new RendaVariavelAnaliseSaidaExemplo()
                }
            };
        }
    }

    public class ObterAnaliseAcoesResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Analise_Carteira_Acoes_Obtida_Com_Sucesso },
                Retorno = new[]
                {
                    new RendaVariavelAnaliseSaidaExemplo(),
                    new RendaVariavelAnaliseSaidaExemplo()
                }
            };
        }
    }

    public class RealizarTransferenciaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ContaMensagem.Transferencia_Realizada_Com_Sucesso },
                Retorno = new TransferenciaSaida(new ContaSaidaExemplo(), new ContaSaida(2, "C/C Inter", TipoConta.ContaCorrente, null, "Banco Inter", "0001", "3805177-0"), DateTime.Now, (int)23.54, "Observação sobre a transferência.")
            };
        }
    }
}
