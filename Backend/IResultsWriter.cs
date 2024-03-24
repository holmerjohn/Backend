namespace Backend
{
    public interface IResultsWriter
    {
        public Task WriteResults(CancellationToken cancellationToken = default);
    }
}
