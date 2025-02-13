using Application.Services.ClientService.Interfaces;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Extensions.Logging;
using Shared.Configuration;
using Shared.Configuration.interfaces;

namespace Application.Services.ClientService
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _repository;
        private readonly IClientRepository _clientRepository;
        private readonly IConfigurationService _configuration;
        private readonly ILogger<CardService> _logger;

        public CardService(ICardRepository repository, IClientRepository clientRepository, IConfigurationService configuration, ILogger<CardService> logger)
        {
            _repository = repository;
            _clientRepository = clientRepository;
            _configuration = configuration;
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

        public async Task<Card?> CreateNewCardFromPos(string cardNumber)
        {
            List<CardAutoRegistrationCondition> conditions = _configuration.GetSettings().CardAutoRegistrationConditions;

            if (conditions.Count == 0)
                return null;

            bool makeCard = false;
            decimal startDiscount = 0;

            foreach (var condition in conditions) 
            {
                if (cardNumber.StartsWith(condition.Prefix)
                    && cardNumber.Length == condition.Length)
                {
                    makeCard = true;
                    startDiscount = condition.StartDiscount;
                    break;
                }
            }

            if (!makeCard)
                return null;
            
            var client = new Client()
            {
                Id = cardNumber,
                Discount = startDiscount,
            };

            var card = new Card()
            {
                ClientId = client.Id,
                Id = cardNumber,
            };

            await _clientRepository.CreateAsync(client);
            await _repository.CreateAsync(card);

            return card;

        }
    }
}
