namespace PlayersBay.Services.Data
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Games;

    public interface IGamesService
    {
        Task<int> CreateAsync(params object[] parameters);

        Task<GameViewModel[]> GetAllGamesAsync();
    }
}
