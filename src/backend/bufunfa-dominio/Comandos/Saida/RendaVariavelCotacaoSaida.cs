using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Dominio.Comandos
{
    /// <summary>
    /// Comando de saída com informações de cotação de uma ação
    /// </summary>
    public class RendaVariavelCotacaoSaida
    {
        public decimal Preco { get; }

        public decimal PercentualMudanca { get; }

        [JsonConverter(typeof(JsonDateFormatConverter), "dd/MM/yyyy")]
        public DateTime? Data { get; }

        public RendaVariavelCotacaoSaida(decimal preco)
        {
            Preco = preco;
        }

        public RendaVariavelCotacaoSaida(decimal preco, decimal percentualMudanca, DateTime? data)
            : this(preco)
        {
            PercentualMudanca = percentualMudanca;
            Data = data;
        }
    }
}
