using Backend.Enums;

namespace Backend.Domain
{
    public abstract class BaseProperty : Entity
    {
        public string Name { get; set; }
        public PropertyType PropertyType { get; set; }
        public string? StringValue { get; set; }
        public decimal? NumberValue { get; set; }

        public void SetPropertyValue(object? value)
        {
            if (value == null)
            {
                PropertyType = PropertyType.Null;
                NumberValue = null;
                StringValue = null;
                return;
            }

            if (value is string)
            {
                PropertyType = PropertyType.String;
                NumberValue = null;
                StringValue = value.ToString();
            }
            else
            {
                PropertyType = PropertyType.Number;
                NumberValue = Convert.ToDecimal(value);
                StringValue = null;
            }

        }
    }
}
