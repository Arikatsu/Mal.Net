using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mal.Net.Utils;

internal class SingleDataConverter<T> : JsonConverter<IEnumerable<T>>
{
    public override IEnumerable<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.StartArray)
        {
            var list = new List<T>();

            while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
            {
                var item = JsonSerializer.Deserialize<T>(ref reader, options);
                list.Add(item!);
            }

            return list;
        }
        
        var singleItem = JsonSerializer.Deserialize<T>(ref reader, options);
        return new List<T> { singleItem! };
    }

    public override void Write(Utf8JsonWriter writer, IEnumerable<T> value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, value.FirstOrDefault(), options);
    }
}