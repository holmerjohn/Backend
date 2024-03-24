using Backend.Enums;
using System.Text.Json;

namespace Backend.Extensions
{
    public static class JsonElementExtensions
    {
        public static PropertyType GetPropertyType(this JsonElement value)
        {
            switch (value.ValueKind)
            {
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return PropertyType.Boolean;
                case JsonValueKind.String:
                    return PropertyType.String;
                case JsonValueKind.Number:
                    return PropertyType.Number;
                default: 
                    return PropertyType.Null;
            }
        }

        public static object? GetValueFromJson(this JsonElement jsonElement)
        {
            switch (jsonElement.ValueKind)
            {
                case JsonValueKind.Null:
                    return null;
                case JsonValueKind.True:
                case JsonValueKind.False:
                    return jsonElement.GetBoolean();
                case JsonValueKind.String:
                    return jsonElement.GetString();
                case JsonValueKind.Number:
                    return jsonElement.GetDecimal();
                default:
                    return null;
            }
        }
    }
}
