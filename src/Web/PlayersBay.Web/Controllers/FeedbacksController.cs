namespace PlayersBay.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Feedbacks;
    using PlayersBay.Services.Data.Utilities;

    public class FeedbacksController : BaseController
    {
        private readonly IFeedbacksService feedbacksService;

        public FeedbacksController(IFeedbacksService feedbacksService)
        {
            this.feedbacksService = feedbacksService;
        }

        [Authorize]
        public IActionResult Create(string offerId)
        {
            this.ViewData["offerId"] = offerId;
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(FeedbackInputModel inputModel)
        {
            this.feedbacksService.CreateAsync(inputModel).GetAwaiter().GetResult();

            return this.RedirectToAction("BoughtOffers", "Offers").WithSuccess(DataConstants.NotificationMessages.Success, DataConstants.NotificationMessages.FeedbackAdded);
        }
    }
}
