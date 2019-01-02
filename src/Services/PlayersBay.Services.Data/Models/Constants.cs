namespace PlayersBay.Services.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class Constants
    {
        public class Offer
        {
            public const int MinPrice = 5;
            public const int MaxPrice = 1000;

            public const string DescriptionLengthError = "Description must be between {2} and {1} symbols";
            public const int DescriptionMinLength = 5;
            public const int DescriptionMaxLength = 500;

            public const string MessageToBuyerLengthError = "Message to buyer must be between {2} and {1} symbols";
            public const int MessageToBuyerMinLength = 5;
            public const int MessageToBuyerMaxLength = 250;
        }
    }
}
