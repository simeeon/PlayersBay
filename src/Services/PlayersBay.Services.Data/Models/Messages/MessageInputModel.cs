namespace PlayersBay.Services.Data.Models.Messages
{
    using System.ComponentModel.DataAnnotations;

    using PlayersBay.Services.Data.Utilities;

    public class MessageInputModel
    {
        [Required]
        [Display(Name = "Send Message:")]
        [StringLength(DataConstants.Message.MessageMaxLength, MinimumLength = DataConstants.Message.MessageMinLength, ErrorMessage = DataConstants.Message.MessageLengthError)]
        public string Message { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }
    }
}
