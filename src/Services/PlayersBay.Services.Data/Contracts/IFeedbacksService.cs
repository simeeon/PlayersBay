namespace PlayersBay.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Models.Feedbacks;

    public interface IFeedbacksService
    {
        Task CreateAsync(FeedbackInputModel inputModel);

        Task<List<FeedbackRating>> GetAllFeedbacksRatingsAsync(string username);
    }
}
