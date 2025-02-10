namespace Domain.FrontolDiscountUnit.Client
{
    using Domain.FrontolDiscountUnit;

    namespace Application.Services.FduService.Interfaces
    {
        public interface IFduService
        {
            Task<FduClient?> GetClientByIdentifierAsync(string identifier);
        }
    }
}
