using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Identity;

namespace PlayersBay.Services.Data.Tests
{
    public class UsersServiceTests : BaseServiceTests
    {
        // User 1
        private readonly string FirstId = Guid.NewGuid().ToString();
        private const string Username = "admin";
        private const string Email = "admin@gmail.com";
        private const string Password = "123";
        // User 2
        private readonly string SecondId = Guid.NewGuid().ToString();
        private const string UsernameTwo = "user";
        private const string EmailTwo = "user@gmail.com";

        private IUsersService UsersServiceMock => this.ServiceProvider.GetRequiredService<IUsersService>();

        protected UserManager<ApplicationUser> UserManager => this.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        protected RoleManager<ApplicationRole> RoleManager => this.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        [Fact]
        public async Task GetAllAsyncReturnsAllUsers()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            var expected = new UserViewModel[]
            {
                new UserViewModel
                {
                    Id = FirstId,
                    Username = Username,
                    Email = Email,
                },
                new UserViewModel
                {
                    Id = SecondId,
                    Username = UsernameTwo,
                    Email = EmailTwo,
                },
            };

            var actual = await this.UsersServiceMock.GetAllUsersAsync();

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Username, elem1.Username);
                    Assert.Equal(expected[0].Email, elem1.Email);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Username, elem2.Username);
                    Assert.Equal(expected[1].Email, elem2.Email);
                });

            Assert.Equal(expected.Length, actual.Length);
        }

        [Fact]
        public async Task MakeModeratorShouldMakeUserWithUserRoleToModerator()
        {
            var user = new ApplicationUser
            {
                Id = FirstId,
                UserName = Username
            };

            await RoleManager.CreateAsync(new ApplicationRole
            {
                Name = Common.GlobalConstants.ModeratorRoleName
            });

            await RoleManager.CreateAsync(new ApplicationRole
            {
                Name = Common.GlobalConstants.UserRoleName
            });

            this.UserManager.CreateAsync(user).GetAwaiter().GetResult();
            this.UserManager.AddPasswordAsync(user, Password).GetAwaiter().GetResult();
            await this.UserManager.AddToRoleAsync(user, Common.GlobalConstants.UserRoleName);

            Assert.False(await this.UserManager.IsInRoleAsync(user, Common.GlobalConstants.ModeratorRoleName));

            await this.UsersServiceMock.MakeModerator(FirstId);

            Assert.True(await this.UserManager.IsInRoleAsync(user, Common.GlobalConstants.ModeratorRoleName));
        }

        [Fact]
        public async Task DemoteFromModeratorShouldMakeUserWithModeratorRoleToUser()
        {
            var user = new ApplicationUser
            {
                Id = FirstId,
                UserName = Username
            };

            await RoleManager.CreateAsync(new ApplicationRole
            {
                Name = Common.GlobalConstants.ModeratorRoleName
            });

            await RoleManager.CreateAsync(new ApplicationRole
            {
                Name = Common.GlobalConstants.UserRoleName
            });

            this.UserManager.CreateAsync(user).GetAwaiter().GetResult();
            this.UserManager.AddPasswordAsync(user, Password).GetAwaiter().GetResult();
            await this.UserManager.AddToRoleAsync(user, Common.GlobalConstants.ModeratorRoleName);

            Assert.False(await this.UserManager.IsInRoleAsync(user, Common.GlobalConstants.UserRoleName));

            await this.UsersServiceMock.DemoteFromModerator(FirstId);

            Assert.True(await this.UserManager.IsInRoleAsync(user, Common.GlobalConstants.UserRoleName));
        }

        [Fact]
        public async Task DeleteByIdOnlyDeletesOneOffer()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);

            var userToDelete = new ApplicationUser
            {
                Id = SecondId,
                UserName = UsernameTwo,
                Email = EmailTwo,
            };

            this.DbContext.Users.Add(userToDelete);
            await this.DbContext.SaveChangesAsync();

            await this.UsersServiceMock.DeleteAsync(userToDelete.Id);

            var expectedDbSetCount = 1;
            Assert.Equal(expectedDbSetCount, this.DbContext.Users.Count());
        }

        private async Task AddTestingUserToDb(string id, string username, string email)
        {
            DbContext.Users.Add(new ApplicationUser
            {
                Id = id,
                UserName = username,
                Email = email,
            });

            await this.DbContext.SaveChangesAsync();
        }
    }

}
