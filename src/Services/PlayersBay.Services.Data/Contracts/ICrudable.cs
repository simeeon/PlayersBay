namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    public interface ICrudable
    {
        Task<int> CreateAsync(params object[] parameters);

        Task<TViewModel> GetViewModelAsync<TViewModel>(int id);

        Task EditAsync(int id, params object[] parameters);

        Task DeleteAsync(int id);
    }
}
