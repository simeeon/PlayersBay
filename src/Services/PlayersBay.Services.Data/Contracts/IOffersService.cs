namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Offers;

    public interface IOffersService
    {
        Task<int> CreateAsync(OfferCreateInputModel inputModel);

        Task<OfferViewModel[]> GetAllOffersAsync(int id);

        Task<OfferViewModel[]> GetMyActiveOffersAsync(string username);

        Task<OfferViewModel[]> GetMySoldOffersAsync(string username);

        Task<OfferViewModel[]> GetMyBoughtOffersAsync(string username);

        Task<TViewModel> GetViewModelAsync<TViewModel>(int id);

        Task EditAsync(OfferToEditViewModel offerToEdit);

        Task DeleteAsync(int id);
    }
}
