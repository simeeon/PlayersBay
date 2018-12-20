namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Services.Data;
    using PlayersBay.Web.Areas.Administrator.InputModels.Games;

    public class GamesController : AdministratorController
    {
        private readonly IGamesService gamesService;

        public GamesController(IGamesService gamesService)
        {
            this.gamesService = gamesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(GamesCreateInputModel gameCreateInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(gameCreateInputModel);
            }

            var fileType = gameCreateInputModel.ImageUrl.ContentType.Split('/')[1];

            if (!this.IsImageTypeValid(fileType))
            {
                return this.View(gameCreateInputModel);
            }

            var gameId = this.gamesService.CreateAsync(
                    gameCreateInputModel.Name,
                    gameCreateInputModel.ImageUrl)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult All()
        {
            var allGamesViewModel = this.gamesService.GetAllGamesAsync()
                .GetAwaiter()
                .GetResult();

            return this.View(allGamesViewModel);
        }
    }
}
