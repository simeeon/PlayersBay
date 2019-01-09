namespace PlayersBay.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Deals;
    using PlayersBay.Services.Data.Utilities;

    public class DealsController : BaseController
    {
        private readonly IDealsService dealsService;

        public DealsController(IDealsService dealsService)
        {
            this.dealsService = dealsService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Purchase(DealInputModel createInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(createInputModel);
            }

            var buyerUsername = this.User.Identity.Name;

            this.dealsService.CreateAsync(buyerUsername, createInputModel).GetAwaiter().GetResult();

            return this.RedirectToAction("BoughtOffers", "Offers").WithSuccess(DataConstants.NotificationMessages.Success, string.Format(DataConstants.NotificationMessages.OfferPurchased, createInputModel.OfferId));
        }
    }
}
