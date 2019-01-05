namespace PlayersBay.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Deals;

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

            var username = this.User.Identity.Name;

            var dealId = this.dealsService.CreateAsync(createInputModel).GetAwaiter()
                .GetResult();

            if (dealId == 0)
            {
                return this.Redirect("/").WithWarning("Failed!", "Not enough funds.");
            }

            return this.Redirect("/").WithSuccess("Success!", $"Offer #{dealId} purchased.");
        }
    }
}
