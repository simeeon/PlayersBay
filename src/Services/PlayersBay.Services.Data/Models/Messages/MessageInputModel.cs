namespace PlayersBay.Services.Data.Models.Messages
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class MessageInputModel
    {
        [Required]
        [StringLength(300)]
        [Display(Name = "Send Message to Seller:")]
        public string Message { get; set; }

        public string SenderName { get; set; }

        public string ReceiverName { get; set; }
    }
}
