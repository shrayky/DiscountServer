using Domain.Entities;

namespace Domain.Repositories
{
    public interface ICardRepository
    {
        Task<Card> ByIdAsync(string id);
        Task<bool> CreateAsync(Card card);
        Task<bool> UpdateAsync(Card card);
        Task<bool> DeleteAsync(Card card);
        Task<bool> DeleteAsync(string id);
        Task<bool> DeleteAllAsync();
        Task<bool> CreateBulkAsync(IEnumerable<Card> cards);
    }
}
