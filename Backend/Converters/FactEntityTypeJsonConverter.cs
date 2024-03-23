using Backend.Enums;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Converters
{
    public class FactEntityTypeJsonConverter : JsonConverter<FactEntityType>
    {
        public override FactEntityType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (Enum.TryParse(reader.GetString(), true, out FactEntityType actionType))
            {
                return actionType;
            }
            else
            {
                return FactEntityType.Unsupported;
            }
        }

        public override void Write(Utf8JsonWriter writer, FactEntityType value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
