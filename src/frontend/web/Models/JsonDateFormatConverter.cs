using Newtonsoft.Json.Converters;

namespace JNogueira.Bufunfa.Web.Models
{
    /// <summary>
    /// Classa para deserialização de datas do formato JSON
    /// </summary>
    public class JsonDateFormatConverter : IsoDateTimeConverter
    {
        public JsonDateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
