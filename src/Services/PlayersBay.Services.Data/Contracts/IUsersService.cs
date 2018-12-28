namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Offers;
    using PlayersBay.Services.Data.Models.Users;

    public interface IUsersService
    {
        Task<UserViewModel[]> GetAllUsersAsync();
    }
}
