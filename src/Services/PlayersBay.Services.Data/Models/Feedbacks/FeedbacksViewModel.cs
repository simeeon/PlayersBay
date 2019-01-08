namespace PlayersBay.Services.Data.Models.Feedbacks
{
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Mapping;

    public class FeedbacksViewModel : IMapFrom<Feedback>
    {
        public FeedbackRating FeedbackRating { get; set; }

        public int OfferId { get; set; }

        public string Content { get; set; }

        public bool HasFeedback { get; set; }
    }
}
