namespace Domain.Exchange
{
    public interface IExchangeLineProcessor
    {
        Task ProcessLineAsync(string line);
        Task ProcessLinesAsync(IEnumerable<string> lines);
    }
}
