namespace PlayersBay.Services.Data.Models.Feedbacks
{
    using System.ComponentModel.DataAnnotations;

    using PlayersBay.Data.Models.Enums;

    public class FeedbackInputModel
    {
        [Required]
        [Display(Name = "Rating")]
        public FeedbackRating FeedbackRating { get; set; }

        public int OfferId { get; set; }

        [Required]
        [Display(Name = "Feedback text")]
        [DataType(DataType.MultilineText)]
        [StringLength(Constants.Feedback.FeedbackMaxLength, MinimumLength = Constants.Feedback.FeedbackMinLength, ErrorMessage = Constants.Feedback.FeedbackLengthError)]
        public string Content { get; set; }
    }
}
