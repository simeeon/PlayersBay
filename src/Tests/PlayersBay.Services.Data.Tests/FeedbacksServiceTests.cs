using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Data.Models.Enums;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Feedbacks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class FeedbacksServiceTests : BaseServiceTests
    {
        // First offer info
        private const int offerId = 1;
        // Feedback 1
        private const FeedbackRating FeedbackRatingEnum = FeedbackRating.Positive;
        private const string FeedbackText = "Sample Feedback text";
        // First offer info
        private const int FirstId = 1;
        private const string OfferTitle = "Title for my offer";
        private const string OfferDescription = "Description for my offer";
        private const decimal OfferPrice = 15.90m;
        private const string OfferMessagetoBuyer = "Message to buyer";
        // User 1
        private readonly string FirstUserId = Guid.NewGuid().ToString();
        private const string Username = "admin";
        private const string Email = "admin@gmail.com";
        private const decimal InitialBalance = 100;
        private const decimal TopUpBalance = 50;

        private IFeedbacksService FeedbacksServiceMock => this.ServiceProvider.GetRequiredService<IFeedbacksService>();

        [Fact]
        public async Task CreateAsyncMakesFeedback()
        {
            await this.AddTestingOfferToDb();

            this.DbContext.Feedbacks.Add(new Feedback
            {
                Content = FeedbackText,
                FeedbackRating = FeedbackRatingEnum,
                OfferId = FirstId,
                HasFeedback = false,
            });
            await this.DbContext.SaveChangesAsync();

            var feedbackInputModel = new FeedbackInputModel
            {
                FeedbackRating = FeedbackRatingEnum,
                Content = FeedbackText,
                OfferId = offerId,
            };

            var feedback = this.DbContext.Feedbacks.FirstOrDefault(u => u.OfferId == offerId);

            Assert.False(feedback.HasFeedback);

            await this.FeedbacksServiceMock.CreateAsync(feedbackInputModel);

            Assert.True(feedback.HasFeedback);

            var expectedFeedbackCount = 2;
            Assert.Equal(expectedFeedbackCount, this.DbContext.Feedbacks.Count());

            Assert.Equal(FeedbackRatingEnum, feedback.FeedbackRating);
            Assert.Equal(FeedbackText, feedback.Content);
            Assert.Equal(expectedFeedbackCount, feedback.Id);
        }

        [Fact]
        public async Task GetAllFeedbacksRatingsAsyncReturnsAllRatingsAsArray()
        {
            await this.AddTestingUserToDb(FirstUserId, Username, Email);

            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                GameId = FirstId,
                Id = FirstId,
                Price = OfferPrice,
                OfferType = OfferType.Items,
                Status = OfferStatus.Completed,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
                SellerId = FirstUserId,
                FeedbackId = FirstId,
            });
            await this.DbContext.SaveChangesAsync();

            var expected = new List<FeedbackRating>
            {
                FeedbackRating.Positive
            };

            var feedback = this.DbContext.Feedbacks.FirstOrDefault(u => u.Id == FirstId);

            feedback.FeedbackRating = FeedbackRating.Positive;
            await this.DbContext.SaveChangesAsync();

            var actual = await this.FeedbacksServiceMock.GetAllFeedbacksRatingsAsync(Username);
            
            Assert.Equal(expected.Count, actual.Count);
        }

        private async Task AddTestingOfferToDb()
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                GameId = FirstId,
                Id = FirstId,
                Price = OfferPrice,
                OfferType = OfferType.Items,
                Status = OfferStatus.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
            });
            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddTestingFeedbackToDb()
        {
            this.DbContext.Feedbacks.Add(new Feedback
            {
                Content = FeedbackText,
                FeedbackRating = FeedbackRatingEnum,
                OfferId = FirstId,
                HasFeedback = true,
            });
            await this.DbContext.SaveChangesAsync();
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
    }
}

