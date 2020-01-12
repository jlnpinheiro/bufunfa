using Newtonsoft.Json.Converters;

namespace JNogueira.Bufunfa.Dominio
{
    public class JsonDateFormatConverter : IsoDateTimeConverter
    {
        public JsonDateFormatConverter(string format)
        {
            DateTimeFormat = format;
        }
    }
}
