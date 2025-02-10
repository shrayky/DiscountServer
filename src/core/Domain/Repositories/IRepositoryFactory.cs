namespace Domain.Repositories
{
    public interface IRepositoryFactory
    {
        IClientRepository CreateClientRepository();
        ICardRepository CreateCardRepository();
    }
}
