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
        [Display(Name = "Commentext")]
        [DataType(DataType.MultilineText)]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Content must be between 5 and 100 symbols")]
        public string Content { get; set; }
    }
}
