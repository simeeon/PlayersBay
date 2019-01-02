using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Games;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class GamesServiceTests : BaseServiceTests
    {
        private const string Diablo = "Diablo";
        private const string CsGo = "CsGo";
        private const string Minecraft = "Minecraft";
        private const string DefaultImage = "http://icons.iconarchive.com/icons/guillendesign/variations-3/256/Default-Icon-icon.png"; 

        private IGamesService GamesServiceMock => this.ServiceProvider.GetRequiredService<IGamesService>();

        [Fact]
        public async Task GetAllAsyncReturnsAllGames()
        {
            this.DbContext.Games.Add(new Game { Id = 1, Name = Diablo, ImageUrl = DefaultImage });
            this.DbContext.Games.Add(new Game { Id = 2, Name = CsGo, ImageUrl = DefaultImage });
            this.DbContext.Games.Add(new Game { Id = 3, Name = Minecraft, ImageUrl = DefaultImage });
            await this.DbContext.SaveChangesAsync();

            var expected = new GameViewModel[]
            {
                new GameViewModel {Id = 1, Name = Diablo, ImageUrl = DefaultImage},
                new GameViewModel {Id = 2, Name = CsGo, ImageUrl = DefaultImage},
                new GameViewModel {Id = 3, Name = Minecraft, ImageUrl = DefaultImage},
            };

            var actual = await this.GamesServiceMock.GetAllGamesAsync();

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Name, elem1.Name);
                    Assert.Equal(expected[0].ImageUrl, elem1.ImageUrl);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Name, elem2.Name);
                    Assert.Equal(expected[1].ImageUrl, elem2.ImageUrl);
                },
                elem3 =>
                {
                    Assert.Equal(expected[2].Id, elem3.Id);
                    Assert.Equal(expected[2].Name, elem3.Name);
                    Assert.Equal(expected[2].ImageUrl, elem3.ImageUrl);
                });
            Assert.Equal(expected.Length, actual.Length);
        }
    }
}
