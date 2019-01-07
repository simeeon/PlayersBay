namespace PlayersBay.Data.Seeding
{
    using System;
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PlayersBay.Common;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Games;
    using PlayersBay.Services.Data.Models.Offers;

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
            var offerService = serviceProvider.GetRequiredService<IOffersService>();
            var gameRepository = serviceProvider.GetRequiredService<IRepository<Game>>();
            var offerRepository = serviceProvider.GetRequiredService<IRepository<Offer>>();

            Seed(dbContext, roleManager, userManager, gamesService, gameRepository, offerService, offerRepository);
        }

        public static void Seed(
            ApplicationDbContext dbContext,
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IGamesService gamesService,
            IRepository<Game> gameRepository,
            IOffersService offerService,
            IRepository<Offer> offerRepository)
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
            SeedOffers(offerService, offerRepository);
        }

        private static void SeedGames(IGamesService gamesService, IRepository<Game> gameRepository)
        {
            var allGames = gameRepository.All();
            if (!allGames.Any())
            {
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "Diablo 2",
                    ImageUrl = "http://media.foxygamer.com/2013/08/diablo_poster13-200x200.jpg",
                })
            .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "Diablo 3",
                    ImageUrl = "http://media.wow-europe.com/events/gamescom-2013/news/11/gamescom_d3-expansion-announcement_facebook-thumb-gl.jpg",
                })
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "Counter-Strike: GO",
                    ImageUrl = "https://d23wybgr07mqxm.cloudfront.net/wp-content/uploads/2016/10/25225911/CSGO-Main-2.png",
                })
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "OSRS",
                    ImageUrl = "https://p.apk4fun.com/bc/2c/56/com.jagex.oldscape.android-logo.jpg",
                })
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "Path of Exile",
                    ImageUrl = "https://steamuserimages-a.akamaihd.net/ugc/541849273143870272/533E218036411B98AF3EC48B01C881B5C6E9AD77/?interpolation=lanczos-none&output-format=jpeg&output-quality=95&fit=inside%7C200%3A200&composite-to=*,*%7C200%3A200&background-color=black",
                })
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "World of Warcraft",
                    ImageUrl = "http://bnetcmsus-a.akamaihd.net/cms/template_resource/fh/FHSCSCG9CXOC1462229977849.png",
                })
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "Fortnite",
                    ImageUrl = "https://fortnitestats.net/assets/img/img2.jpg",
                })
                .GetAwaiter()
                .GetResult();
                gamesService.CreateAsync(new GamesCreateInputModel()
                {
                    Name = "Black Desert",
                    ImageUrl = "https://cdn.gracza.pl/galeria/gry13/grupy/13149.jpg",
                })
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
                ApplicationUser user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                };

                IdentityResult result = userManager.CreateAsync(user, "admin").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, GlobalConstants.AdministratorRoleName).Wait();
                }
            }
        }

        private static void SeedOffers(IOffersService offerService, IRepository<Offer> offerRepository)
        {
            var sellerUsername = "admin";
            var allOffers = offerRepository.All();
            if (!allOffers.Any())
            {
                for (int i = 0; i < 15; i++)
                {
                    var offer = new OfferCreateInputModel
                    {
                        GameId = 1,
                        Description = $"Nice Item {i}. Buy it now!",
                        Duration = 7,
                        ImageUrl = null,
                        MessageToBuyer = "Hello buyer",
                        OfferType = Models.Enums.OfferType.Items,
                        Price = 14.90m + i,
                        Title = $"My title {i}",
                    };

                    offerService.CreateAsync(sellerUsername, offer)
                        .GetAwaiter()
                        .GetResult();
                }
            }
        }
    }
}
