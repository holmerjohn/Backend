using Backend.Enums;

namespace Backend.Domain.Facts
{
    public class FactStatus : Entity
    {
        public string FactId { get; set; }
        public virtual Fact Fact { get; set; }

        public FactEntityType EntityType { get; set; }
        public string EntityId { get; set; }

        public string Name { get; set; }
        public bool Status { get; set; }
    }
}

