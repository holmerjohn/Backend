namespace Backend
{
    public class BackendConfiguration
    {
        public bool EnableSensitiveSqlLogging { get; init; }
        public string? ConnectionString { get; init; }
        public string CurrentDirectory { get; set; }
    }
}