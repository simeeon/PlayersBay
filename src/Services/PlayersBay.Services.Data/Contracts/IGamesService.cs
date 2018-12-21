namespace PlayersBay.Services.Data
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Games;

    public interface IGamesService : ICrudable
    {
        Task<GameViewModel[]> GetAllGamesAsync();
    }
}
