using JNogueira.Bufunfa.Api.ViewModels;
using JNogueira.Bufunfa.Dominio.Comandos;
using JNogueira.Bufunfa.Dominio.Resources;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace JNogueira.Bufunfa.Api.Swagger.Exemplos
{
    public class ParcelaRequestExemplo : IExamplesProvider<ParcelaViewModel>
    {
        public ParcelaViewModel GetExamples()
        {
            return new ParcelaViewModel
            {
                Data = DateTime.Now.Date,
                Valor = (decimal)134.21,
                Observacao = "Observação da parcela"
            };
        }
    }

    public class ParcelaSaidaExemplo : ParcelaSaida
    {
        public ParcelaSaidaExemplo()
            : base (1, 1, null, DateTime.Now, (decimal)12.34, 1, false, false, null, "Parcela 1", null, new { Id = 1, IdConta = 1, Conta = "Conta X", CartaoCredito = string.Empty, CategoriaTipo = "D", CategoriaCaminho = "DÉBITO » Outros", Pessoa = "Uber", QuantidadeParcelas = 10 })
        {

        }
    }

    public class CadastrarParcelaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Cadastrada_Com_Sucesso },
                Retorno = new ParcelaSaidaExemplo()
            };
        }
    }

    public class LancarParcelaRequestExemplo : IExamplesProvider<LancarParcelaViewModel>
    {
        public LancarParcelaViewModel GetExamples()
        {
            return new LancarParcelaViewModel
            {
                Data = DateTime.Today,
                Valor = (decimal?)120.23,
                Observacao = "Observação do lançamento da parcela"
            };
        }
    }

    public class LancarParcelaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Lancada_Com_Sucesso },
                Retorno = new ParcelaSaidaExemplo()
            };
        }
    }

    public class DescartarParcelaRequestExemplo : IExamplesProvider<DescartarParcelaViewModel>
    {
        public DescartarParcelaViewModel GetExamples()
        {
            return new DescartarParcelaViewModel
            {
                MotivoDescarte = "Descrição do motivo do descarte da parcela"
            };
        }
    }

    public class DescartarParcelaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Descartada_Com_Sucesso },
                Retorno = new ParcelaSaidaExemplo()
            };
        }
    }

    public class AlterarParcelaRequestExemplo : IExamplesProvider<ParcelaViewModel>
    {
        public ParcelaViewModel GetExamples()
        {
            return new ParcelaViewModel
            {
                Data = DateTime.Now.Date,
                Valor = (decimal)134.21,
                Observacao = "Observação da parcela"
            };
        }
    }

    public class AlterarParcelaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Alterada_Com_Sucesso },
                Retorno = new ParcelaSaidaExemplo()
            };
        }
    }

    public class ExcluirParcelaResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Excluida_Com_Sucesso },
                Retorno = new ParcelaSaidaExemplo()
            };
        }
    }

    public class ObterParcelaPorIdResponseExemplo : IExamplesProvider<Saida>
    {
        public Saida GetExamples()
        {
            return new Saida
            {
                Sucesso = true,
                Mensagens = new[] { ParcelaMensagem.Parcela_Encontrada_Com_Sucesso },
                Retorno = new ParcelaSaidaExemplo()
            };
        }
    }
}
