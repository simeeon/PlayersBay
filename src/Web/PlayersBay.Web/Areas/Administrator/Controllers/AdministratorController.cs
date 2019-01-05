namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Web.Controllers;

    [Area("Administrator")]
    [Authorize(Roles = Common.GlobalConstants.AdministratorRoleName)]
    public class AdministratorController : BaseController
    {
    }
}
