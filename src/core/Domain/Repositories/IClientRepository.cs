using Domain.Entities;

namespace Domain.Repositories
{
    public interface IClientRepository
    {
        Task<Client> GetAsync(string id);
        Task<Client> ByIdAsync(string id);
        Task<Client> ByPhoneAsync(string phone);
        Task<bool> CreateAsync(Client client);
        Task<bool> UpdateAsync(Client client);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteAsync(Client client);
        Task<bool> DeleteAllAsync();
        Task<bool> CreateBulkAsync(IEnumerable<Client> clients);
    }
}
