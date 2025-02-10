namespace Domain.Exchange
{
    public interface IExchangeRepositoryFactory
    {
        IExchangeLineProcessor? GetProcessor(string section);
    }
}
