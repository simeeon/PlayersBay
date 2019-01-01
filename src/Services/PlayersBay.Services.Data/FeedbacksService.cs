namespace PlayersBay.Services.Data
{
    using System.Threading.Tasks;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Feedbacks;

    public class FeedbacksService : IFeedbacksService
    {
        private readonly IRepository<Feedback> feedbacksRepository;

        public FeedbacksService(
            IRepository<Feedback> feedbacksRepository)
        {
            this.feedbacksRepository = feedbacksRepository;
        }

        public async Task CreateAsync(FeedbackInputModel inputModel)
        {
            var feedback = new Feedback
            {
                FeedbackRating = inputModel.FeedbackRating,
                Content = inputModel.Content,
                HasFeedback = true,
                OfferId = inputModel.OfferId,
            };

            this.feedbacksRepository.Add(feedback);
            await this.feedbacksRepository.SaveChangesAsync();
        }
    }
}
