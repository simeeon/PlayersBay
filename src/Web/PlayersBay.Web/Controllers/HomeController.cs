namespace PlayersBay.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Services.Data.Contracts;

    public class HomeController : BaseController
    {
        public HomeController(IGamesService gamesService)
        {
            this.GamesService = gamesService;
        }

        protected IGamesService GamesService { get; }

        public IActionResult Index()
        {
            var allGamesViewModel = this.GamesService.GetAllGamesAsync().GetAwaiter().GetResult();

            return this.View(allGamesViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() => this.View();
    }
}
