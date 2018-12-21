namespace PlayersBay.Data.Models
{
    using PlayersBay.Data.Common.Models;
    using PlayersBay.Data.Models.Enums;

    public class Feedback : BaseDeletableModel<int>
    {
        public FeedbackRating FeedbackRating { get; set; }

        public string Content { get; set; }

        public int OfferId { get; set; }

        public Offer Offer { get; set; }

        public bool HasFeedback { get; set; }
    }
}
