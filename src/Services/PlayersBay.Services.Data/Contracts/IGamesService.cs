namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Games;

    public interface IGamesService : ICrudable
    {
        Task<GameViewModel[]> GetAllGamesAsync();
    }
}
