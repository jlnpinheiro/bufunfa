using JNogueira.Utilzao;
using Newtonsoft.Json;
using System;

namespace JNogueira.Bufunfa.Infraestrutura.Integracoes.AlphaVantage.Comandos
{
    public class GlobalQuote
    {
        [JsonProperty("Global Quote")]
        public GlobalQuoteParams Params { get; set; }

        public static GlobalQuote Obter(string json)
        {
            return !string.IsNullOrEmpty(json)
                ? JsonConvert.DeserializeObject<GlobalQuote>(json)
                : throw new Exception("A saida da API foi nula ou vazia.");
        }
    }

    public class GlobalQuoteParams
    {
        [JsonProperty("01. symbol")]
        public string Symbol { get; set; }

        [JsonProperty("02. open")]
        public string Open { get; set; }

        [JsonProperty("03. high")]
        public string High { get; set; }

        [JsonProperty("04. low")]
        public string Low { get; set; }

        [JsonProperty("05. price")]
        public string Price { get; set; }

        [JsonProperty("06. volume")]
        public string Volume { get; set; }

        [JsonProperty("07. latest trading day")]
        public string LatestTradingDay { get; set; }

        [JsonProperty("08. previous close")]
        public string PreviousClose { get; set; }

        [JsonProperty("09. change")]
        public string Change { get; set; }

        [JsonProperty("10. change percent")]
        public string ChangePercent { get; set; }
    }

    public class GlobalQuoteSaida
    {
        public DateTime? LatestTradingDay { get; }

        public decimal ChangePercent { get; }

        public decimal Price { get; }

        public GlobalQuoteSaida(GlobalQuoteParams globalQuoteParams)
        {
            if (globalQuoteParams == null)
                return;

            this.LatestTradingDay = globalQuoteParams?.LatestTradingDay?.ConverterDataPorFormato("yyyy-MM-dd");
            this.ChangePercent    = Convert.ToDecimal(globalQuoteParams?.ChangePercent?.Replace("%", string.Empty), new System.Globalization.CultureInfo("en-US"));
            this.Price            = Convert.ToDecimal(globalQuoteParams?.Price, new System.Globalization.CultureInfo("en-US"));
        }
    }
}
