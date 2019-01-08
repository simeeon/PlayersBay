namespace PlayersBay.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Feedbacks;

    public class FeedbacksService : IFeedbacksService
    {
        private readonly IRepository<Feedback> feedbacksRepository;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Offer> offersRepository;

        public FeedbacksService(
            IRepository<Feedback> feedbacksRepository,
            IRepository<ApplicationUser> usersRepository,
            IRepository<Offer> offersRepository)
        {
            this.feedbacksRepository = feedbacksRepository;
            this.usersRepository = usersRepository;
            this.offersRepository = offersRepository;
        }

        public async Task CreateAsync(FeedbackInputModel inputModel)
        {
            var feedback = await this.feedbacksRepository.All().FirstOrDefaultAsync(f => f.OfferId == inputModel.OfferId);

            feedback.Content = inputModel.Content;
            feedback.FeedbackRating = inputModel.FeedbackRating;
            feedback.HasFeedback = true;

            this.feedbacksRepository.Update(feedback);
            await this.feedbacksRepository.SaveChangesAsync();
        }

        public async Task<List<FeedbackRating>> GetAllFeedbacksRatingsAsync(string username)
        {
            var user = this.usersRepository.All().FirstOrDefault(u => u.UserName == username);

            var allFeedbacks = await this.feedbacksRepository.All().ToArrayAsync();

            var allOffers = await this.offersRepository.All().ToArrayAsync();

            var userOffersId = allOffers.Where(o => o.Seller == user && o.Status == OfferStatus.Completed).Select(i => i.FeedbackId).ToList();

            var feedbacks = await this.feedbacksRepository.All().Where(f => userOffersId.Contains(f.Id) && f.IsDeleted == false).Select(x => x.FeedbackRating)
                .ToListAsync();

            return feedbacks;
        }
    }
}
