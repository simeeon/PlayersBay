namespace PlayersBay.Services.Data.Utilities
{
    public class DataConstants
    {
        public class Transfer
        {
            public const int MinAmount = 1;
            public const int MaxAmount = 2000;
        }

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

        public const string InvalidImageFormat = "The image format should be jpg/jpeg or png";

        public const string JpgFormat = "jpg";

        public const string JpegFormat = "jpeg";

        public const string PngFormat = "png";

        public const string InvalidWithdrawalAmount = "Cannot withdraw ${0}, when {1} balance is ${2}.";

        public const string InsufficientFundsError = "Cannot afford this offer. You need ${0}, when balance is ${1}.";

        public const string NullReferenceOfferId = "Offer with id {0} was not found.";

        public const string NullReferenceGameId = "Game with id {0} was not found.";

        public const string InvalidDeleteMessage = "You cannot delete message if you are not the sender or receiver";

        public class NotificationMessages
        {
            public const string Info = "Done";

            public const string Success = "Success";

            public const string Fail = "Fail";

            public const string DebitRequest = "Debit request for ${0} created.";

            public const string WithdrawalRequest = "Withdrawal request for ${0} created.";

            public const string GameCreated = "Game {0} created.";

            public const string GameEdited = "Game {0} edited.";

            public const string GameDeleted = "Game {0} deleted.";

            public const string OfferPurchased = "Offer #{0} purchased.";

            public const string FeedbackAdded = "Feedback added";

            public const string OfferCreated = "Offer #{0} created.";

            public const string OfferEdited = "Offer #{0} edited.";

            public const string OfferDeleted = "Offer #{0} deleted.";

            public const string MessageSent = "Message to user: {0} sent.";

            public const string MessageDeleted = "Message with id #{0} deleted.";

            public const string UserPromoted = "Role is now moderator for id {0}.";

            public const string UserDemoted = "Role is now user for id {0}.";
        }
    }
}
