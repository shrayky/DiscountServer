using Domain.Entities;
using Domain.Exchange;
using Domain.Repositories;
using System.Globalization;

namespace Exchange.LineProcessors
{
    public class ClientLineProcessor : IExchangeLineProcessor
    {
        private readonly IClientRepository _repository;

        public ClientLineProcessor(IRepositoryFactory repositoryFactory)
        {
            _repository = repositoryFactory.CreateClientRepository();
        }

        public async Task ProcessLineAsync(string line)
        {
            var fields = line.Split(';');

            DateTime birthday = ToDateTeime(fields[3]);

            List<Client> loaded = new();

            var client = new Client
            {
                Id = fields[0],
                Name = fields[1],
                Sex = fields[2],
                Birthday = birthday,
                Phone = fields[4],
                Discount = decimal.Parse(fields[5], CultureInfo.InvariantCulture),
                Bonus = decimal.Parse(fields[6], CultureInfo.InvariantCulture)
            };

            loaded.Add(client);

            await _repository.CreateAsync(client);
        }

        public async Task ProcessLinesAsync(IEnumerable<string> lines)
        {
            var clients = lines.Select(line =>
            {
                var fields = line.Split(';');
                return new Client
                {
                    Id = fields[0],
                    Name = fields[1],
                    Sex = fields[2],
                    Birthday = ToDateTeime(fields[3]),
                    Phone = fields[4],
                    Discount = decimal.Parse(fields[5], CultureInfo.InvariantCulture),
                    Bonus = decimal.Parse(fields[6], CultureInfo.InvariantCulture)
                };
            }).ToList();

            await _repository.CreateBulkAsync(clients);
        }

        private DateTime ToDateTeime(string dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString) || dateString.Replace(".", "").Trim().Length == 0)
                return DateTime.MinValue;

            // формат dd.MM.yy
            if (dateString.Length == 8)
            {
                var year = int.Parse(dateString.Substring(6, 2));
                year = year < 50 ? 2000 + year : 1900 + year;
                dateString = $"{dateString.Substring(0, 6)}{year}";
            }

            return DateTime.ParseExact(dateString, "dd.MM.yyyy", CultureInfo.InvariantCulture);
        }
    }
}
