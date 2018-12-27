namespace PlayersBay.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Services.Data.Utilities;

    public class BaseController : Controller
    {
        protected bool IsImageTypeValid(string fileType)
        {
            return fileType == DataConstants.JpgFormat || fileType == DataConstants.PngFormat || fileType == DataConstants.JpegFormat;
        }
    }
}
