using Application.Services.ClientService.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.ClientService
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _repository;
        private readonly ILogger<ClientService> _logger;

        public ClientService(IClientRepository clientRepository, ILogger<ClientService> logger)
        {
            _repository = clientRepository;
            _logger = logger;
        }

        public async Task<Client> ByIdAsync(string id)
        {
            return await _repository.ByIdAsync(id);
        }

        public async Task<Client> ByPhoneAsync(string phone)
        {
            return await _repository.ByPhoneAsync(phone);
        }

        public async Task<bool> CreateClientAsync(Client client)
        {
            return await _repository.CreateAsync(client);
        }

        public async Task<Client> GetByIdAsync(string id)
        {
            return await _repository.ByIdAsync(id);
        }
    }
}
