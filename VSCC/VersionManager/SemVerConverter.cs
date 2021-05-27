namespace VSCC.VersionManager
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    public class SemVerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string) || objectType == typeof(SemanticVersioning.Version);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new SemanticVersioning.Version(0, 0, 0);
            }
            else
            {
                JToken token = JToken.Load(reader);
                return new SemanticVersioning.Version(token.Value<string>());
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken token = JToken.FromObject(((SemanticVersioning.Version)value).ToString());
            token.WriteTo(writer);
        }
    }
}
