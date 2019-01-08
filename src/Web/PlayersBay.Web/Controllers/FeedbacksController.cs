namespace PlayersBay.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Feedbacks;

    public class FeedbacksController : BaseController
    {
        private readonly IFeedbacksService feedbacksService;
        private readonly IRepository<Offer> offerRepository;

        public FeedbacksController(
            IFeedbacksService feedbacksService,
            IRepository<Offer> offerRepository)
        {
            this.feedbacksService = feedbacksService;
            this.offerRepository = offerRepository;
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
            this.feedbacksService.CreateAsync(inputModel)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("BoughtOffers", "Offers").WithSuccess("Success", "Feedback added.");
        }
    }
}
