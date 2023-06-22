using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Converters;

class BirthDateJsonConverter : JsonConverter<Int64>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (DateOnly.TryParse(reader.GetString(), out DateOnly value))
        {
            return value.ToDateTime(TimeOnly.MinValue).Ticks;
        }

        throw new FormatException();
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options) => writer.WriteStringValue(new DateTime(value).ToString("d"));

}