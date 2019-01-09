namespace PlayersBay.Services.Data.Models.Feedbacks
{
    using System.ComponentModel.DataAnnotations;

    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Utilities;

    public class FeedbackInputModel
    {
        [Required]
        [Display(Name = "Rating")]
        public FeedbackRating FeedbackRating { get; set; }

        public int OfferId { get; set; }

        [Required]
        [Display(Name = "Feedback text")]
        [DataType(DataType.MultilineText)]
        [StringLength(DataConstants.Feedback.FeedbackMaxLength, MinimumLength = DataConstants.Feedback.FeedbackMinLength, ErrorMessage = DataConstants.Feedback.FeedbackLengthError)]
        public string Content { get; set; }
    }
}
