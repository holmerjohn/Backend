using Backend.Domain;

namespace Backend.Extensions
{
    public static class EntityExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T entity) where T : Entity
        {
            return new List<T>() { entity }.AsEnumerable();
        }
    }
}
