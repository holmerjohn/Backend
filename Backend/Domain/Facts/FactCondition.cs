using Backend.Enums;

namespace Backend.Domain.Facts
{
    public class FactCondition : Entity
    {
        public string FactId { get; set; }

        public string Field { get; set; }
        public ConditionComparator Comparator { get; set; }
        public PropertyType? PropertyType { get; set; }
        public string? Value { get; set; }

        public bool IsSatisfiedByProperty(BaseProperty property)
        {
            if (property == null) return false;

            switch (Comparator)
            {
                case ConditionComparator.Exists:
                    {
                        return ((property.PropertyType != Enums.PropertyType.Null) || 
                            (!string.IsNullOrEmpty(property.StringValue) || property.NumberValue.HasValue));
                    }
                case ConditionComparator.Equals:
                    {
                        if (PropertyType != property.PropertyType)
                            return false;

                        switch (PropertyType)
                        {
                            case Enums.PropertyType.Number:
                                {
                                    return Convert.ToDecimal(Value) == Convert.ToDecimal(property.NumberValue);
                                }
                            case Enums.PropertyType.String:
                                {
                                    return string.Equals(Value, property.StringValue, StringComparison.CurrentCultureIgnoreCase);
                                }
                            default:
                                return false;
                        }
                    }
                default:
                    return false;
            }
        }

    }
}
