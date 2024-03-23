using Backend.Enums;
using Backend.Extensions;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Backend.Domain
{
    public class EntityAction
    {
        public EntityActionType Action { get; set; }
        public string? LoanIdentifier { get; set; }
        public string? BorrowerIdentifier { get; set; }
        public PropertyType PropertyType { get; set; }
        public string? Field { get; set; }
        public object? Value { get; set; }
    }
}
