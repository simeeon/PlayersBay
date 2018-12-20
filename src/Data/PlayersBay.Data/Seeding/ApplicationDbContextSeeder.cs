namespace PlayersBay.Data.Seeding
{
    using System;
    using System.Linq;

    using PlayersBay.Common;
    using PlayersBay.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PlayersBay.Services.Data;
    using PlayersBay.Data.Repositories;
    using PlayersBay.Data.Common.Repositories;

    public static class ApplicationDbContextSeeder
    {
        public static void Seed(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (serviceProvider == null)
            {
                throw new ArgumentNullException(nameof(serviceProvider));
            }

            var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var gamesService = serviceProvider.GetRequiredService<IGamesService>();
            var gameRepository = serviceProvider.GetRequiredService<IRepository<Game>>();

            Seed(dbContext, roleManager, userManager, gamesService, gameRepository);
        }

        public static void Seed(
            ApplicationDbContext dbContext,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IGamesService gamesService,
            IRepository<Game> gameRepository)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            if (roleManager == null)
            {
                throw new ArgumentNullException(nameof(roleManager));
            }

            SeedRoles(roleManager);
            SeedUsers(userManager);
            SeedGames(gamesService, gameRepository);
        }

        private static void SeedGames(IGamesService gamesService, IRepository<Game> gameRepository)
        {
            var allGames = gameRepository.All();
            if (!allGames.Any())
            {
                gamesService.CreateAsync(
                    "Diablo 2",
                    "http://www.oldpcgaming.net/wp-content/uploads/2016/06/Snap182_1.jpg")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "Diablo 3",
                    "https://en.wikipedia.org/wiki/Diablo_III#/media/File:Diablo_III_cover.png")
                .GetAwaiter()
                .GetResult();
            }
        }

        private static void SeedRoles(RoleManager<ApplicationRole> roleManager)
        {
            SeedRole(GlobalConstants.AdministratorRoleName, roleManager);
            SeedRole(GlobalConstants.ModeratorRoleName, roleManager);
            SeedRole(GlobalConstants.UserRoleName, roleManager);
        }

        private static void SeedRole(string roleName, RoleManager<ApplicationRole> roleManager)
        {
            var role = roleManager.FindByNameAsync(roleName).GetAwaiter().GetResult();
            if (role == null)
            {
                var result = roleManager.CreateAsync(new ApplicationRole(roleName)).GetAwaiter().GetResult();

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            if (userManager.FindByNameAsync("admin").Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = "admin";
                user.Email = "admin@admin.com";
                user.UserName = "Admin";

                IdentityResult result = userManager.CreateAsync(user, "admin").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName).Wait();
                }
            }
        }
    }
}
