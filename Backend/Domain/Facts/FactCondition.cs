using Backend.Enums;

namespace Backend.Domain.Facts
{
    public class FactCondition : Entity
    {
        public string FactId { get; set; }
        public virtual Fact Fact { get; set; }

        public string Field { get; set; }
        public ConditionComparator Comparator { get; set; }
        public PropertyType? PropertyType { get; set; }
        public string? Value { get; set; }
    }
}
