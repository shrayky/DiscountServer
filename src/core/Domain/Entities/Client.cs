using Domain.Common;

namespace Domain.Entities
{
    public class Client : IHasId
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Sex { get; set; } = string.Empty;
        public DateTime Birthday { get; set; } 
        public string Phone { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal Bonus { get; set; }
    }
}
