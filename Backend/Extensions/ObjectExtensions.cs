using Backend.Enums;

namespace Backend.Extensions
{
    public static class ObjectExtensions
    {
        public static PropertyType GetPropertyType(this object? value)
        {
            switch (value)
            {
                case null:
                    return PropertyType.Null;
                case string _:
                    return PropertyType.String;
                case bool _:
                    return PropertyType.Boolean;
                default:
                    return PropertyType.Number;
            }
        }

        public static object? GetValueFromObject(this object? value)
        {
            switch (value)
            {
                case null:
                    return null;
                case string _:
                    return value.ToString();
                case bool _:
                    return Convert.ToBoolean(value);
                default:
                    return Convert.ToDecimal(value);
            }
        }
    }
}
