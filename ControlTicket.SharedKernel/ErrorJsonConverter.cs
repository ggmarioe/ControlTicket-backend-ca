using System.Text.Json;
using System.Text.Json.Serialization;

namespace ControlTicket.SharedKernel;

public sealed class ErrorJsonConverter : JsonConverter<Error>
{
    public override Error Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
    {
        if (value == Error.None)
        {
            writer.WriteRawValue("{}");
        }
        else
        {
            writer.WriteStartObject();
            writer.WriteString("code", value.Code);
            writer.WriteString("description", value.Description);
            writer.WriteNumber("type", (int)value.Type);
            writer.WriteEndObject();
        }
    }
}
