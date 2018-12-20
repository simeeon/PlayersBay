namespace PlayersBay.Data.Seeding
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PlayersBay.Common;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data;

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
                    "http://media.foxygamer.com/2013/08/diablo_poster13-200x200.jpg")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "Diablo 3",
                    "http://media.wow-europe.com/events/gamescom-2013/news/11/gamescom_d3-expansion-announcement_facebook-thumb-gl.jpg")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "Counter-Strike: GO",
                    "https://d23wybgr07mqxm.cloudfront.net/wp-content/uploads/2016/10/25225911/CSGO-Main-2.png")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "OSRS",
                    "https://p.apk4fun.com/bc/2c/56/com.jagex.oldscape.android-logo.jpg")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "Path of Exile",
                    "https://steamuserimages-a.akamaihd.net/ugc/541849273143870272/533E218036411B98AF3EC48B01C881B5C6E9AD77/?interpolation=lanczos-none&output-format=jpeg&output-quality=95&fit=inside%7C200%3A200&composite-to=*,*%7C200%3A200&background-color=black")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "World of Warcraft",
                    "http://bnetcmsus-a.akamaihd.net/cms/template_resource/fh/FHSCSCG9CXOC1462229977849.png")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "Fortnite",
                    "https://fortnitestats.net/assets/img/img2.jpg")
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(
                    "Black Desert",
                    "https://cdn.gracza.pl/galeria/gry13/grupy/13149.jpg")
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
