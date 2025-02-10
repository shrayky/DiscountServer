using Domain.Entities;

namespace Application.Services.ClientService.Interfaces
{
    public interface IClientService
    {
        Task<Client> GetByIdAsync(string id);
        Task<Client> ByPhoneAsync(string phone);
        Task<Client> ByIdAsync(string id);
        Task<bool> CreateClientAsync(Client client);
    }
}
