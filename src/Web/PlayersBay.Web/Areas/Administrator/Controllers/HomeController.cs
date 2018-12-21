namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Services.Data.Utilities;
    using PlayersBay.Web.Controllers;

    [Area("Administrator")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : AdministratorController
    {
        public IActionResult Index()
        {
            return this.View();
        }
    }
}
