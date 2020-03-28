namespace VSCC.State.Adapters
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;

    public class SortingMethodAdapter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => true;

        // Previous versions used a string which can't work with default MD5 checksums due to localization.
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => reader.TokenType == JsonToken.String ? 0 : (object)JToken.Load(reader).ToObject<int>();

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => JToken.FromObject(value).WriteTo(writer);
    }
}
