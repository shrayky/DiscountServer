using Application.Services.ClientService.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.ClientService
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _repository;
        private readonly ILogger<CardService> _logger;

        public CardService(ICardRepository repository, ILogger<CardService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<Card> ByIdAsync(string id)
        {
            return await _repository.ByIdAsync(id);
        }

        public async Task<bool> CreateCardAsync(Card card)
        {
            return await _repository.CreateAsync(card);
        }
    }
}
