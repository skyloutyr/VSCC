using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSCC.State.Adapters
{
    public class SortingMethodAdapter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Previous versions used a string which can't work with default MD5 checksums due to localization.
            if (reader.TokenType == JsonToken.String)
            {
                return 0;
            }
            
            return JToken.Load(reader).ToObject<int>();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => JToken.FromObject(value).WriteTo(writer);
    }
}
