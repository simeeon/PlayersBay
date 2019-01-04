namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Deals;

    public interface IDealsService
    {
        Task<int> CreateAsync(DealInputModel inputModel);

        Task TopUpAsync(TopUpInputModel inputModel);
    }
}
