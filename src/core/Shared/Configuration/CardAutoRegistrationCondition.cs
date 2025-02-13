namespace Shared.Configuration
{
    public class CardAutoRegistrationCondition
    {
        public string Prefix { get; set; } = string.Empty;
        public int Length { get; set; } = 0;
        public decimal StartDiscount { get; set; } = 0;
    }
}
