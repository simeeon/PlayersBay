using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Deals;
using PlayersBay.Services.Data.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class DealsServiceTests : BaseServiceTests
    {
        private const decimal InitialBalance = 100;
        private const decimal TopUpBalance = 50;
        // User 1
        private readonly string FirstId = Guid.NewGuid().ToString();
        private const string Username = "admin";
        private const string Email = "admin@gmail.com";
        // User 2
        private readonly string SecondId = Guid.NewGuid().ToString();
        private const string UsernameTwo = "user";
        private const string EmailTwo = "user@gmail.com";
        // First offer info
        private const int offerId = 1;
        private const string OfferTitle = "Title for my offer";
        private const string OfferDescription = "Description for my offer";
        private const decimal OfferPrice = 15.90m;
        private const string OfferMessagetoBuyer = "Message to buyer";

        private IDealsService DealsServiceMock => this.ServiceProvider.GetRequiredService<IDealsService>();

        [Fact]
        public async Task CreateAsyncReturnsCreatedDeal()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);
            await this.AddTestingOfferToDb();

            var dealInputModel = new DealInputModel
            {
                BuyerName = Username,
                SellerName = UsernameTwo,
                OfferId = offerId
            };

            await this.DealsServiceMock.CreateAsync(Username, dealInputModel);

            var offersCount = this.DbContext.Offers.Count();

            Assert.Equal(offersCount, offerId);
        }

        [Fact]
        public async Task CreateAsyncReturnsInvalidOperationExceptionWhenInsufficientFunds()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            var offerPrice = InitialBalance + 1;
            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                Id = offerId,
                Price = offerPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.OfferStatus.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
            });

            await this.DbContext.SaveChangesAsync();

            var dealInputModel = new DealInputModel
            {
                BuyerName = Username,
                SellerName = UsernameTwo,
                OfferId = offerId
            };

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                this.DealsServiceMock.CreateAsync(Username, dealInputModel));

            Assert.Equal(string.Format(DataConstants.InsufficientFundsError, offerPrice, InitialBalance), exception.Message);
        }

        private async Task AddTestingUserToDb(string id, string username, string email)
        {
            DbContext.Users.Add(new ApplicationUser
            {
                Id = id,
                UserName = username,
                Email = email,
                Balance = InitialBalance,
            });

            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddTestingOfferToDb()
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                Id = offerId,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.OfferStatus.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
            });

            await this.DbContext.SaveChangesAsync();
        }
    }
}

