namespace Application.Services.FrontolDiscountUnit
{
    using Domain.FrontolDiscountUnit.Client.Application.Services.FduService.Interfaces;
    using Domain.FrontolDiscountUnit.Client.Domain.FrontolDiscountUnit;
    using global::Application.Services.ClientService.Interfaces;
    using Microsoft.Extensions.Logging;

    namespace Application.Services.FduService
    {
        public class FduService : IFduService
        {
            private readonly IClientService _clientService;
            private readonly ICardService _cardService;
            private readonly ILogger<FduService> _logger;

            public FduService(
                IClientService clientService,
                ICardService cardService,
                ILogger<FduService> logger)
            {
                _clientService = clientService;
                _cardService = cardService;
                _logger = logger;
            }

            public async Task<FduClient?> GetClientByIdentifierAsync(string identifier)
            {
                string? clientId = await GetClientIdAsync(identifier);
                
                if (clientId == null)
                    return null;

                var client = await _clientService.GetByIdAsync(clientId);
                
                if (client == null)
                    return null;

                return new FduClient
                {
                    Id = client.Id,
                    Name = client.Name,
                    Birthday = client.Birthday.ToString("yyyy-MM-dd"),
                    Sex = client.Sex.ToLower(),
                    Phone = client.Phone ?? "",
                    Email = "",
                    Enabled = true,
                    Notify_email = false,
                    Notify_sms = false,
                    Bonus = client.Bonus * 100,
                    Discount = client.Discount
                };
            }

            private async Task<string?> GetClientIdAsync(string identifier)
            {
                if (identifier.Length == 11)
                {
                    var clientByPhone = await _clientService.ByPhoneAsync(identifier);
                    if (clientByPhone != null)
                        return clientByPhone.Id;
                }

                // Если по телефону не нашли, ищем по карте
                var card = await _cardService.ByIdAsync(identifier);

                if (card == null)
                    card = await _cardService.CreateNewCardFromPos(identifier);


                return card?.ClientId;
            }
        }
    }
}
