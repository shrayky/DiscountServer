using Domain.Entities;
using Domain.Exchange;
using Domain.Repositories;

namespace Exchange.LineProcessors
{
    public class CardLineProcessor : IExchangeLineProcessor
    {
        private readonly ICardRepository _repository;

        public CardLineProcessor(IRepositoryFactory repositoryFactory)
        {
            _repository = repositoryFactory.CreateCardRepository();
        }

        public async Task ProcessLinesAsync(IEnumerable<string> lines)
        {
            var cards = lines.Select(line =>
            {
                var fields = line.Split(';');
                return new Card
                {
                    Id = fields[0],
                    ClientId = fields[1]
                };
            }).ToList();

            await _repository.CreateBulkAsync(cards);
        }

        public async Task ProcessLineAsync(string line)
        {
            var fields = line.Split(';');
            var card = new Card
            {
                Id = fields[0],
                ClientId = fields[1]
            };

            await _repository.CreateAsync(card);
        }
    }
}
