namespace PlayersBay.Services.Data
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Messages;
    using PlayersBay.Services.Data.Models.Users;
    using PlayersBay.Services.Mapping;

    public class UsersService : IUsersService
    {
        public UsersService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IRepository<ApplicationUser> usersRepository)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.usersRepository = usersRepository;
        }

        private readonly IRepository<ApplicationUser> usersRepository;

        private UserManager<ApplicationUser> UserManager { get; }

        private SignInManager<ApplicationUser> SignInManager { get; }

        public async Task<UserViewModel[]> GetAllUsersAsync()
        {
            var users = await this.usersRepository
                .All()
                .To<UserViewModel>()
                .ToArrayAsync();

            return users;
        }
    }
}
