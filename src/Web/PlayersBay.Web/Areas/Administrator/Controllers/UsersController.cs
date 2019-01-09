namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Utilities;

    public class UsersController : AdministratorController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult All()
        {
            var allGamesViewModel = this.usersService.GetAllUsersAsync().GetAwaiter().GetResult();

            return this.View(allGamesViewModel);
        }

        public IActionResult MakeUserModerator(string id)
        {
            this.usersService.MakeModerator(id).GetAwaiter().GetResult();

            return this.RedirectToAction("All", "Users").WithSuccess(DataConstants.NotificationMessages.Success, string.Format(DataConstants.NotificationMessages.UserPromoted, id));
        }

        public IActionResult DemoteUserFromModerator(string id)
        {
            this.usersService.DemoteFromModerator(id).GetAwaiter().GetResult();

            return this.RedirectToAction("All", "Users").WithSuccess(DataConstants.NotificationMessages.Success, string.Format(DataConstants.NotificationMessages.UserDemoted, id));
        }

        public IActionResult Delete(string id)
        {
            this.usersService.DeleteAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("All", "Users");
        }
    }
}
