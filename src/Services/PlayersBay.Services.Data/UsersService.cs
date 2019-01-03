namespace PlayersBay.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Users;
    using PlayersBay.Services.Mapping;

    public class UsersService : IUsersService
    {
        private readonly IRepository<ApplicationUser> usersRepository;

        private readonly UserManager<ApplicationUser> userManager;

        public UsersService(IRepository<ApplicationUser> usersRepository, UserManager<ApplicationUser> userManager)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
        }

        public async Task<UserViewModel[]> GetAllUsersAsync()
        {
            var users = await this.usersRepository
                .All()
                .To<UserViewModel>()
                .ToArrayAsync();

            return users;
        }

        public async Task<string> MakeModerator(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || await this.userManager.IsInRoleAsync(user, Common.GlobalConstants.ModeratorRoleName))
            {
                throw new NullReferenceException();
            }

            await this.userManager.RemoveFromRoleAsync(user, Common.GlobalConstants.UserRoleName);
            await this.userManager.AddToRoleAsync(user, Common.GlobalConstants.ModeratorRoleName);

            return $"{user.UserName} promoted";
        }

        public async Task<string> DemoteFromModerator(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);

            if (user == null || await this.userManager.IsInRoleAsync(user, Common.GlobalConstants.UserRoleName))
            {
                throw new NullReferenceException();
            }

            await this.userManager.RemoveFromRoleAsync(user, Common.GlobalConstants.ModeratorRoleName);
            await this.userManager.AddToRoleAsync(user, Common.GlobalConstants.UserRoleName);

            return $"{user.UserName} demoted.";
        }

        public async Task DeleteAsync(string id)
        {
            var user = this.usersRepository.All().FirstOrDefault(d => d.Id == id);
            user.IsDeleted = true;

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
