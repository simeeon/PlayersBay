namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
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
    }
}
