using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Feedbacks;
using PlayersBay.Services.Data.Models.Games;
using PlayersBay.Services.Data.Models.Offers;
using PlayersBay.Services.Data.Utilities;
using PlayersBay.Services.Mapping;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class OffersServiceTests : BaseServiceTests
    {
        private const int FirstId = 1;
        private const string GameName = "Diablo";
        private const string OfferTitle = "Title for my offer";
        private const string OfferDescription = "Description for my offer";
        private const decimal OfferPrice = 15.90m;
        private const string OfferMessagetoBuyer = "Message to buyer";

        private const string DefaultImage = "http://icons.iconarchive.com/icons/guillendesign/variations-3/256/Default-Icon-icon.png";
        private const string NewImage = "newImage.png";

        public OffersServiceTests()
        {
            // AutoMapper
            AutoMapper.Mapper.Reset();
        }

        private IOffersService OffersServiceMock => this.ServiceProvider.GetRequiredService<IOffersService>();

        [Fact]
        public async Task GetViewModelByIdAsyncReturnsCorrectViewModel()
        {
            await this.AddTestingGameToDb();
            await this.AddTestingOfferToDb();

            var expected = this.DbContext.Offers.OrderBy(r => r.CreatedOn);

            var actual = await this.OffersServiceMock.GetViewModelAsync<OfferToEditViewModel>(FirstId);

            Assert.IsType<OfferToEditViewModel>(actual);
            Assert.Collection(expected,
                elem1 =>
                {
                    Assert.Equal(expected.First().Id, actual.Id);
                    Assert.Equal(expected.First().MessageToBuyer, actual.MessageToBuyer);
                    Assert.Equal(expected.First().OfferType, actual.OfferType);
                    Assert.Equal(expected.First().Price, actual.Price);
                    Assert.Equal(expected.First().Title, actual.Title);
                    Assert.Equal(expected.First().Description, actual.Description);
                    Assert.Equal(expected.First().Status, actual.Status);
                    Assert.Equal(expected.First().GameId, actual.GameId);
                    Assert.Equal(expected.First().ImageUrl, actual.ImageUrl);
                });
        }

        private async Task AddTestingGameToDb()
        {
            this.DbContext.Games.Add(new Game
            {
                Id = FirstId,
                Name = GameName,
            });
            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddTestingOfferToDb()
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                GameId = FirstId,
                Id = FirstId,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            });
            await this.DbContext.SaveChangesAsync();
        }
    }
}
