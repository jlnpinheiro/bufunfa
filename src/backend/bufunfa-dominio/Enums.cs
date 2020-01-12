using System;
using System.ComponentModel;
using System.Linq;

namespace JNogueira.Bufunfa.Dominio
{
    /// <summary>
    /// Tipo de categoria
    /// </summary>
    public static class TipoCategoria
    {
        public static readonly string Credito = "C";
        public static readonly string Debito = "D";
    }

    /// <summary>
    /// Métodos de pagamento
    /// </summary>
    public enum MetodoPagamento
    {
        Cheque        = 1,
        [Description("Débito")]
        Debito        = 2,
        [Description("Depósito")]
        Deposito      = 3,
        [Description("Transferência")]
        Transferencia = 4,
        Dinheiro      = 5
    }

    /// <summary>
    /// Periodicidade
    /// </summary>
    public enum Periodicidade
    {
        Mensal     = 1,
        Trimestral = 3,
        Semestral  = 6,
        Anual      = 12
    }

    /// <summary>
    /// Status de uma parcela
    /// </summary>
    public enum StatusParcela
    {
        Aberta,
        Fechada
    }

    /// <summary>
    /// Tipo da conta
    /// </summary>
    public enum TipoConta
    {
        [Description("Conta corrente")]
        ContaCorrente = 1,
        [Description("Poupança")]
        Poupanca = 2,
        [Description("Renda fixa")]
        RendaFixa = 3,
        [Description("Renda variável")]
        RendaVariavel = 4
    }

    /// <summary>
    /// Tipo de categorias especiais
    /// </summary>
    public enum TipoCategoriaEspecial
    {
        [Description("TRANSFERÊNCIA » Origem")]
        TransferenciaOrigem        = 1,
        [Description("TRANSFERÊNCIA » Destino")]
        TransferenciaDestino       = 2,
        [Description("Pagamento de fatura de cartão")]
        PagamentoFaturaCartao      = 3,
        [Description("Juros ou dividendos")]
        JurosDividendos = 4,
        [Description("Compra de ações")]
        CompraAcoes = 5,
        [Description("Venda de ações")]
        VendaAcoes = 6,
        [Description("Impostos")]
        Impostos = 7,
    }

    public static class ExtensionMethods
    {
        public static string ObterDescricao(this Enum member)
        {
            if (member == null)
                return null;

            var fieldInfo = member.GetType().GetField(member.ToString());

            if (fieldInfo == null)
                return null;

            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).ToList();

            return attributes.Any() ? ((DescriptionAttribute)attributes.First()).Description : member.ToString();
        }
    }
}
