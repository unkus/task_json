using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Models;

namespace Converters;

class GenderJsonConverter : JsonConverter<Gender>
{
    public override Gender Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if(Enum.TryParse(reader.GetString(), out Gender value))
        {
            return value;
        }

        throw new FormatException();
    }

    public override void Write(Utf8JsonWriter writer, Gender value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
    
}