namespace VSCC.VersionManager
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    public class SemVerRangeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(string) || objectType == typeof(SemVer.Range);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return new SemVer.Range("*");
            }
            else
            {
                JToken token = JToken.Load(reader);
                return new SemVer.Range(token.Value<string>());
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            JToken token = JToken.FromObject(((SemVer.Range)value).ToString());
            token.WriteTo(writer);
        }
    }
}
