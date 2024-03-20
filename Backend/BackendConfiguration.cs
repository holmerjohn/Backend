namespace Backend
{
    public class BackendConfiguration
    {
        public bool EnableSensitiveSqlLogging { get; init; }
        public string? ConnectionString { get; init; }
    }
}