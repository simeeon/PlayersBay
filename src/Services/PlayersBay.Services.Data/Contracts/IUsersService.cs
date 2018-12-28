namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Offers;
    using PlayersBay.Services.Data.Models.Users;

    public interface IUsersService
    {
        Task<UserViewModel[]> GetAllUsersAsync();

        Task<string> MakeModerator(string id);

        Task<string> DemoteFromModerator(string id);

        Task DeleteAsync(string id);
    }
}
