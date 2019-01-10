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
        // Offer
        private const int OfferGameId = 1;
        private const string OfferDescription = "This account has warrior - leveled and geared! Buy it now and you won't regret it!";
        private const int OfferDuration = 7;
        private const string OfferMessageToBuyer = "Hi, username: Warrior -> Password: pass";
        private const decimal OfferPrice = 14.90m;
        private const string OfferTitle = "This account has warrior - Leveled and geared! Buy it now and you won't regret it!";

        // Game 1
        private const string GameName1 = "Diablo 2";
        private const string GameImageUrl1 = "http://media.foxygamer.com/2013/08/diablo_poster13-200x200.jpg";

        // Game 2
        private const string GameName2 = "Diablo 3";
        private const string GameImageUrl2 = "http://media.wow-europe.com/events/gamescom-2013/news/11/gamescom_d3-expansion-announcement_facebook-thumb-gl.jpg";

        // Game 3
        private const string GameName3 = "Counter-Strike: GO";
        private const string GameImageUrl3 = "https://d23wybgr07mqxm.cloudfront.net/wp-content/uploads/2016/10/25225911/CSGO-Main-2.png";

        // Game 4
        private const string GameName4 = "OSRS";
        private const string GameImageUrl4 = "https://p.apk4fun.com/bc/2c/56/com.jagex.oldscape.android-logo.jpg";

        // Game 5
        private const string GameName5 = "Path of Exile";
        private const string GameImageUrl5 = "https://steamuserimages-a.akamaihd.net/ugc/541849273143870272/533E218036411B98AF3EC48B01C881B5C6E9AD77/?interpolation=lanczos-none&output-format=jpeg&output-quality=95&fit=inside%7C200%3A200&composite-to=*,*%7C200%3A200&background-color=black";

        // Game 6
        private const string GameName6 = "World of Warcraft";
        private const string GameImageUrl6 = "http://bnetcmsus-a.akamaihd.net/cms/template_resource/fh/FHSCSCG9CXOC1462229977849.png";

        // Game 7
        private const string GameName7 = "Fortnite";
        private const string GameImageUrl7 = "https://fortnitestats.net/assets/img/img2.jpg";

        // Game 8
        private const string GameName8 = "Black Desert";
        private const string GameImageUrl8 = "https://cdn.gracza.pl/galeria/gry13/grupy/13149.jpg";

        private static readonly GamesCreateInputModel[] GamesToSeed = new GamesCreateInputModel[]
        {
            new GamesCreateInputModel
            {
                Name = GameName1,
                ImageUrl = GameImageUrl1,
            },
            new GamesCreateInputModel
            {
                Name = GameName2,
                ImageUrl = GameImageUrl2,
            },
            new GamesCreateInputModel
            {
                Name = GameName3,
                ImageUrl = GameImageUrl3,
            },
            new GamesCreateInputModel
            {
                Name = GameName4,
                ImageUrl = GameImageUrl4,
            },
            new GamesCreateInputModel
            {
                Name = GameName5,
                ImageUrl = GameImageUrl5,
            },
            new GamesCreateInputModel
            {
                Name = GameName6,
                ImageUrl = GameImageUrl6,
            },
            new GamesCreateInputModel
            {
                Name = GameName7,
                ImageUrl = GameImageUrl7,
            },
            new GamesCreateInputModel
            {
                Name = GameName8,
                ImageUrl = GameImageUrl8,
            },
        };

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
                for (int i = 0; i < GamesToSeed.Count(); i++)
                {
                    gamesService.CreateAsync(GamesToSeed[i]).GetAwaiter().GetResult();
                }
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
            if (userManager.FindByNameAsync(GlobalConstants.AdministratorUerName).Result == null)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = GlobalConstants.AdministratorUerName,
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
            var sellerUsername = GlobalConstants.AdministratorUerName;
            var allOffers = offerRepository.All();
            if (!allOffers.Any())
            {
                for (int i = 0; i < 15; i++)
                {
                    var offer = new OfferCreateInputModel
                    {
                        GameId = OfferGameId,
                        Description = OfferDescription,
                        Duration = OfferDuration,
                        ImageUrl = null,
                        MessageToBuyer = OfferMessageToBuyer,
                        OfferType = Models.Enums.OfferType.Account,
                        Price = OfferPrice + i,
                        Title = OfferTitle,
                    };

                    offerService.CreateAsync(sellerUsername, offer).GetAwaiter().GetResult();
                }
            }
        }
    }
}
