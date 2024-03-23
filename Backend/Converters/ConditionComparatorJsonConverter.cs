using Backend.Domain.Facts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Converters
{
    public class ConditionComparatorJsonConverter : JsonConverter<ConditionComparator>
    {
        public override ConditionComparator Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (Enum.TryParse(reader.GetString(), true, out ConditionComparator actionType))
            {
                return actionType;
            }
            else
            {
                return ConditionComparator.Unsupported;
            }
        }

        public override void Write(Utf8JsonWriter writer, ConditionComparator value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString().ToLower());
        }
    }
}
