namespace PlayersBay.Services.Data.Utilities
{
    public class DataConstants
    {
        public const string InvalidImageFormat = "The image format should be jpg/jpeg or png";

        public const string JpgFormat = "jpg";

        public const string JpegFormat = "jpeg";

        public const string PngFormat = "png";

        public const string InvalidWithdrawalAmount = "Cannot withdraw ${0}, when {1} balance is ${2}.";

        public const string InsufficientFundsError = "Cannot afford this offer. You need ${0}, when balance is ${1}.";

        public const string NullReferenceOfferId = "Offer with id {0} was not found.";

        public const string NullReferenceGameId = "Game with id {0} was not found.";

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
        }
    }
}
