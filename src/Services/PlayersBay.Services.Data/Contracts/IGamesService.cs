namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Games;

    public interface IGamesService
    {
        Task<int> CreateAsync(GamesCreateInputModel inputModel);

        Task DeleteAsync(int id);

        Task EditAsync(GameToEditViewModel editViewModel);

        Task<GameViewModel[]> GetAllGamesAsync();

        Task<TViewModel> GetViewModelAsync<TViewModel>(int id);
    }
}
