namespace PlayersBay.Services.Data.Models.Messages
{
    using System.ComponentModel.DataAnnotations;

    public class MessageInputModel
    {
        [Required]
        [Display(Name = "Send Message:")]
        [StringLength(Constants.Message.MessageMaxLength, MinimumLength = Constants.Message.MessageMinLength, ErrorMessage = Constants.Message.MessageLengthError)]
        public string Message { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }
    }
}
