namespace PlayersBay.Web.Controllers
{

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Transfers;
    using System.Linq;

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
        public IActionResult All(string username)
        {
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

            this.transfersService
                .CreateDepositRequestAsync(inputModel)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/").WithSuccess("Success!", $"Debit request for {inputModel.Amount} created.");
        }

        [Authorize]
        [HttpPost]
        public IActionResult Withdraw(TransferInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            this.transfersService
                .CreateWithdrawalRequestAsync(inputModel)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/").WithSuccess("Success!", $"Withdrawal request for {inputModel.Amount} created.");
        }
    }
}
