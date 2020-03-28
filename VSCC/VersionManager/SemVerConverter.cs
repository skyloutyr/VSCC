using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace VSCC.VersionManager
{
    public class SemVerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string) || objectType == typeof(SemVer.Version);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new SemVer.Version(0, 0, 0);
            }
            else
            {
                JToken token = JToken.Load(reader);
                return new SemVer.Version(token.Value<string>());
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken token = JToken.FromObject(((SemVer.Version)value).ToString());
            token.WriteTo(writer);
        }
    }
}
