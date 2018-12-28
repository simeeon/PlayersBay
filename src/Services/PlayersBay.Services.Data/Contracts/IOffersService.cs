namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Offers;

    public interface IOffersService
    {
        Task<int> CreateAsync(string username, params object[] parameters);

        Task<OfferViewModel[]> GetAllOffersAsync(int id);

        Task<OfferViewModel[]> GetMyActiveOffersAsync(string username);

        Task<OfferDetailsViewModel> GetDetailsAsync(int id);

        Task<TViewModel> GetViewModelAsync<TViewModel>(int id);

        Task EditAsync(OfferToEditViewModel offerToEdit);

        Task DeleteAsync(int id);
    }
}
