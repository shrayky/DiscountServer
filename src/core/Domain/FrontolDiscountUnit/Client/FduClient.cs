namespace Domain.FrontolDiscountUnit.Client
{
    namespace Domain.FrontolDiscountUnit
    {
        public class FduClient
        {
            public string Id { get; set; } = string.Empty;
            public string Name { get; set; } = string.Empty;
            public string Birthday { get; set; } = string.Empty;
            public string Sex { get; set; } = string.Empty;
            public string Phone { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public bool Enabled { get; set; }
            public bool Notify_email { get; set; }
            public bool Notify_sms { get; set; }
            public decimal Bonus { get; set; }
            public decimal Discount { get; set; }
        }
    }
}
