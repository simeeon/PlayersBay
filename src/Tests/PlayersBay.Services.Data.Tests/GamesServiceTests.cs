using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Feedbacks;
using PlayersBay.Services.Data.Models.Games;
using PlayersBay.Services.Data.Utilities;
using PlayersBay.Services.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class GamesServiceTests : BaseServiceTests
    {
        private const int TestGameId = 1;
        private const string Diablo = "Diablo";
        private const string CsGo = "CsGo";
        private const string Minecraft = "Minecraft";
        private const string DefaultImage = "http://icons.iconarchive.com/icons/guillendesign/variations-3/256/Default-Icon-icon.png";
        private const string NewImage = "newImage.png";

        public GamesServiceTests()
        {
            // AutoMapper
            AutoMapper.Mapper.Reset();
            AutoMapperConfig.RegisterMappings(typeof(FeedbackInputModel).GetTypeInfo().Assembly);
        }

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

        [Fact]
        public async Task CreateAsyncReturnsCreatedGame()
        {
            var expected = new GameViewModel[]
            {
                new GameViewModel {Id = 1, Name = Diablo, ImageUrl = DefaultImage},
            };

            var actual = await this.GamesServiceMock.CreateAsync(Diablo, DefaultImage);

            Assert.Equal(actual, expected[0].Id);
        }

        [Fact]
        public async Task DeleteByIdOnlyDeletesOneGame()
        {
            await this.AddTestingGameToDb();

            var secondGame = new Game()
            {
                Id = 2,
                Name = Minecraft,
                ImageUrl = DefaultImage,
            };
            var gameToDelete = new Game
            {
                Id = 3,
                Name = "To be deleted",
                ImageUrl = DefaultImage,
            };
            this.DbContext.Games.Add(secondGame);
            this.DbContext.Games.Add(gameToDelete);
            await this.DbContext.SaveChangesAsync();

            await this.GamesServiceMock.DeleteAsync(gameToDelete.Id);

            var expectedDbSetCount = 2;
            Assert.Equal(expectedDbSetCount, this.DbContext.Games.Count());
        }

        [Fact]
        public async Task EditAsyncEditsGameWhenImageStaysTheSame()
        {
            await this.AddTestingGameToDb();

            this.DbContext.Games.Add(new Game { Id = 2, Name = "TestEditGame" });
            await this.DbContext.SaveChangesAsync();

            var newName = Minecraft;
            var newGameId = 2;

            Assert.NotEqual(newName, this.DbContext.Games.Find(1).Name);
            Assert.NotEqual(newGameId, this.DbContext.Games.Find(1).Id);

            var gameEditViewModel = new GameToEditViewModel()
            {
                Id = 1,
                Name = newName,
                NewImage = null,
            };

            await this.GamesServiceMock.EditAsync(gameEditViewModel);

            Assert.Equal(newName, this.DbContext.Games.Find(1).Name);
        }

        [Fact]
        public async Task EditAsyncEditsGameImage()
        {
            this.DbContext.Games.Add(new Game
            {
                Id = TestGameId,
                Name = Diablo,
                ImageUrl = "https://static.icy-veins.com/images/heroes/og-image-hero-diablo.jpg",
            });
            await this.DbContext.SaveChangesAsync();

            using (var stream = File.OpenRead(NewImage))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "png",
                };

                var gameToEditViewModel = new GameToEditViewModel()
                {
                    Id = TestGameId,
                    Name = Diablo,
                    NewImage = file,
                };

                await this.GamesServiceMock.EditAsync(gameToEditViewModel);

                ApplicationCloudinary.DeleteImage(ServiceProvider.GetRequiredService<Cloudinary>(), gameToEditViewModel.Name);
            }

            Assert.NotEqual(NewImage, this.DbContext.Games.Find(TestGameId).ImageUrl);
        }

        [Fact]
        public async Task GetViewModelByIdAsyncReturnsCorrectViewModel()
        {
            await this.AddTestingGameToDb();

            var expected = this.DbContext.Games.OrderBy(r => r.CreatedOn);

            var actual = await this.GamesServiceMock.GetViewModelAsync<GameToEditViewModel>(TestGameId);

            Assert.IsType<GameToEditViewModel>(actual);
            Assert.Collection(expected,
                elem1 =>
                {
                    Assert.Equal(expected.First().Id, actual.Id);
                    Assert.Equal(expected.First().Name, actual.Name);
                });
        }

        private async Task AddTestingGameToDb()
        {
            this.DbContext.Games.Add(new Game
            {
                Id = TestGameId,
                Name = Diablo,
            });
            await this.DbContext.SaveChangesAsync();
        }
    }
}
