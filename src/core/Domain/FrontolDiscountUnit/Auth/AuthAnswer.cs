namespace Domain.FrontolDiscountUnit.Auth
{
    public class AuthAnswer
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public long Expired { get; set; }
        public string Signature { get; set; } = string.Empty;
    }
}
