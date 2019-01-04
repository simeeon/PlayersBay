using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Transactions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class TransactionsServiceTests : BaseServiceTests
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

        private ITransactionsService TransactionsServiceMock => this.ServiceProvider.GetRequiredService<ITransactionsService>();

        [Fact]
        public async Task CreateAsyncReturnsCreatedTransaction()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);
            await this.AddTestingOfferToDb();

            var transactionInputModel = new TransactionInputModel
            {
                BuyerName = Username,
                SellerName = UsernameTwo,
                OfferId = offerId
            };

            var actual = await this.TransactionsServiceMock.CreateAsync(transactionInputModel);

            Assert.Equal(actual, offerId);
        }

        [Fact]
        public async Task TopUpAsyncAddsFundsToUserBalance()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            var topUpInputModel = new TopUpInputModel
            {
                Amount = TopUpBalance,
                Username = Username,
            };

            await this.TransactionsServiceMock.TopUpAsync(topUpInputModel);

            var user = this.DbContext.Users.FirstOrDefault(u => u.UserName == Username);

            var expectedBalance = InitialBalance + TopUpBalance;
            Assert.Equal(expectedBalance, user.Balance);
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
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
            });
            await this.DbContext.SaveChangesAsync();
        }
    }
}

