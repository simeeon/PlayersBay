namespace PlayersBay.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Transfers;
    using PlayersBay.Services.Data.Utilities;

    public class TransfersController : BaseController
    {
        private readonly ITransfersService transfersService;
        private readonly IRepository<Transfer> transferRepository;

        public TransfersController(
            ITransfersService transfersService,
            IRepository<Transfer> transferRepository)
        {
            this.transfersService = transfersService;
            this.transferRepository = transferRepository;
        }

        [Authorize]
        public IActionResult All()
        {
            var username = this.User.Identity.Name;

            var transfers = this.transfersService.GetAllTransfersAsync(username)
                .GetAwaiter()
                .GetResult();

            return this.View(transfers);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Deposit(TransferInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var username = this.User.Identity.Name;

            this.transfersService.CreateDepositRequestAsync(username, inputModel).GetAwaiter().GetResult();

            return this.RedirectToAction("All", "Transfers")
                .WithInfo(DataConstants.NotificationMessages.Info, string.Format(DataConstants.NotificationMessages.DebitRequest, inputModel.Amount));
        }

        [Authorize]
        [HttpPost]
        public IActionResult Withdraw(TransferInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var username = this.User.Identity.Name;

            this.transfersService
                .CreateWithdrawalRequestAsync(username, inputModel)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("All", "Transfers").WithInfo(DataConstants.NotificationMessages.Info, string.Format(DataConstants.NotificationMessages.WithdrawalRequest, inputModel.Amount));
        }
    }
}
