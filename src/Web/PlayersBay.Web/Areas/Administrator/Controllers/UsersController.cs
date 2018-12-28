namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data.Contracts;

    public class UsersController : AdministratorController
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public IActionResult All()
        {
            var allGamesViewModel = this.usersService.GetAllUsersAsync()
                .GetAwaiter()
                .GetResult();

            return this.View(allGamesViewModel);
        }

        [Authorize(Roles = Common.GlobalConstants.AdministratorRoleName)]
        public IActionResult MakeUserModerator(string id)
        {
            var data = this.usersService.MakeModerator(id).Result;

            return this.RedirectToAction("All", "Users").WithSuccess("Success!", data);
        }

        [Authorize(Roles = Common.GlobalConstants.AdministratorRoleName)]
        public IActionResult DemoteUserFromModerator(string id)
        {
            var data = this.usersService.DemoteFromModerator(id).Result;

            return this.RedirectToAction("All", "Users").WithSuccess("Success!", data);
        }

        [Authorize(Roles = Common.GlobalConstants.AdministratorRoleName)]

        public IActionResult Delete(string id)
        {
            this.usersService.DeleteAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("All", "Users");
        }
    }
}
