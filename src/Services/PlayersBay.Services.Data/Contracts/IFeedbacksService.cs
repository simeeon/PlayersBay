namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Feedbacks;

    public interface IFeedbacksService
    {
        Task CreateAsync(FeedbackInputModel inputModel);
    }
}
