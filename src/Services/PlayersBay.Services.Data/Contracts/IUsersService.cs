namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Users;

    public interface IUsersService
    {
        Task<UserViewModel[]> GetAllUsersAsync();

        Task MakeModerator(string id);

        Task DemoteFromModerator(string id);

        Task DeleteAsync(string id);
    }
}
