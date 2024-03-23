using Backend.Enums;

namespace Backend.Domain.Facts
{
    public class Fact : Entity
    {
        
        public string Name { get; set; }
        public FactEntityType EntityType { get; set; }
        
        public virtual ICollection<FactCondition> Conditions { get; set; } = new List<FactCondition>();
        public static Fact CreateFact(string name)
        {
            return new Fact()
            {
                Id = Guid.NewGuid().ToString(),
                Name = name
            };
        }

    }
}
