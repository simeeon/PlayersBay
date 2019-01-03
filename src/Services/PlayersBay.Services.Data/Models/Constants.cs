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

            public const string TitleLengthError = "Title must be between {2} and {1} symbols";
            public const int TitleMinLength = 5;
            public const int TitleMaxLength = 100;

            public const string MessageToBuyerLengthError = "Message to buyer must be between {2} and {1} symbols";
            public const int MessageToBuyerMinLength = 5;
            public const int MessageToBuyerMaxLength = 250;
        }

        public class Game
        {
            public const string GameLengthError = "Game name must be between {2} and {1} symbols";
            public const int GameMinLength = 3;
            public const int GameMaxLength = 30;
        }

        public class Feedback
        {
            public const string FeedbackLengthError = "Feedback must be between {2} and {1} symbols";
            public const int FeedbackMinLength = 5;
            public const int FeedbackMaxLength = 100;
        }

        public class Message
        {
            public const string MessageLengthError = "Message must be between {2} and {1} symbols";
            public const int MessageMinLength = 5;
            public const int MessageMaxLength = 200;
        }
    }
}
