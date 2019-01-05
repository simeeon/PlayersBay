namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data.Contracts;

    public class TransfersController : AdministratorController
    {
        private readonly ITransfersService transfersService;

        public TransfersController(ITransfersService transfersService)
        {
            this.transfersService = transfersService;
        }

        public IActionResult DepositRequests()
        {
            var allDepositRequestsViewModel = this.transfersService.GetAllDepositRequestsAsync()
                .GetAwaiter()
                .GetResult();

            return this.View(allDepositRequestsViewModel);
        }

        public IActionResult WithdrawalRequests()
        {
            var allDepositRequestsViewModel = this.transfersService.GetAllWithdrawalRequestsAsync()
                .GetAwaiter()
                .GetResult();

            return this.View(allDepositRequestsViewModel);
        }

        public IActionResult ApproveTransfer(int transferId)
        {
            this.transfersService.ApproveTransferAsync(transferId)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/").WithSuccess("Success!", "Transaction approved.");
        }

        public IActionResult DeclineTransfer(int transferId)
        {
            this.transfersService.DeclineTransferAsync(transferId)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/").WithInfo("Declined!", "Transaction declined.");
        }
    }
}
