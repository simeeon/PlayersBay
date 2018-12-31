namespace PlayersBay.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Transactions;

    public class TransactionsController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly IRepository<Offer> offerRepository;
        private readonly ITransactionsService transactionsService;

        public TransactionsController(
            IOffersService offersService,
            IRepository<Offer> offerRepository,
            ITransactionsService transactionsService)
        {
            this.offersService = offersService;
            this.offerRepository = offerRepository;
            this.transactionsService = transactionsService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Purchase(TransactionInputModel createInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(createInputModel);
            }

            var username = this.User.Identity.Name;

            var transactionId = this.transactionsService.CreateAsync(createInputModel).GetAwaiter()
                .GetResult();

            if (transactionId == 0)
            {
                return this.Redirect("/").WithWarning("Failed!", "Not enough funds.");
            }

            return this.Redirect("/").WithSuccess("Success!", $"Offer #{transactionId} purchased.");
        }
    }
}
