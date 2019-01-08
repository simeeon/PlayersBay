namespace PlayersBay.Web.Views.ViewComponents
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Services.Data.Contracts;

    [ViewComponent(Name = "RatingList")]
    public class RatingListViewComponent : ViewComponent
    {
        private readonly IFeedbacksService feedbacksService;

        public RatingListViewComponent(IFeedbacksService feedbacksService)
        {
            this.feedbacksService = feedbacksService;
        }

        public async Task<string> InvokeAsync(string username)
        {
            var feedbackRatings = await this.feedbacksService.GetAllFeedbacksRatingsAsync(username);
            var feedbacksCount = feedbackRatings.Count();
            var sum = 0;

            foreach (var item in feedbackRatings)
            {
                sum += (int)item;
            }

            return $"{sum} /{feedbacksCount} votes/";
        }
    }
}
