using Domain.Entities;

namespace Application.Services.ClientService.Interfaces
{
    public interface ICardService
    {
        Task<Card> ByIdAsync(string id);
        Task<bool> CreateCardAsync(Card card);
    }

}
