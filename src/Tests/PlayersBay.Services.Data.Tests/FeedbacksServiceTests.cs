using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models.Enums;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Feedbacks;
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

        private IFeedbacksService FeedbacksServiceMock => this.ServiceProvider.GetRequiredService<IFeedbacksService>();

        [Fact]
        public async Task CreateAsyncMakesFeedback()
        {
            var feedbackInputModel = new FeedbackInputModel
            {
                FeedbackRating = FeedbackRatingEnum,
                Content = FeedbackText,
                OfferId = offerId,
            };

            await this.FeedbacksServiceMock.CreateAsync(feedbackInputModel);

            var feedback = this.DbContext.Feedbacks.FirstOrDefault(u => u.OfferId == offerId);

            var expectedFeedbackCount = 1;
            Assert.Equal(expectedFeedbackCount, this.DbContext.Feedbacks.Count());

            Assert.Equal(FeedbackRatingEnum, feedback.FeedbackRating);
            Assert.Equal(FeedbackText, feedback.Content);
            Assert.Equal(expectedFeedbackCount, feedback.Id);
        }
    }
}

