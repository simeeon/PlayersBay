namespace PlayersBay.Services.Data.Models.Feedbacks
{
    using PlayersBay.Data.Models.Enums;

    public class FeedbacksViewModel
    {
        public FeedbackRating FeedbackRating { get; set; }

        public int OfferId { get; set; }

        public string Content { get; set; }

        public bool HasFeedback { get; set; }
    }
}
