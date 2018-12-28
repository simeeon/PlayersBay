namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Games;
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

            return this.Redirect("/").WithSuccess("Success!", "Game created.");
        }

        public IActionResult Edit(int id)
        {
            var gameToEdit = this.gamesService.GetViewModelAsync<GameToEditViewModel>(id)
                .GetAwaiter()
                .GetResult();
            if (gameToEdit == null)
            {
                return this.Redirect("/");
            }

            return this.View(gameToEdit);
        }

        [HttpPost]
        public IActionResult Edit(GameToEditViewModel gameToEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(gameToEditViewModel);
            }

            if (gameToEditViewModel.NewImage != null)
            {
                var fileType = gameToEditViewModel.NewImage.ContentType.Split('/')[1];
                if (!this.IsImageTypeValid(fileType))
                {
                    return this.View(gameToEditViewModel);
                }
            }

            var id = gameToEditViewModel.Id;
            this.gamesService.EditAsync(
                id,
                gameToEditViewModel.Name,
                gameToEditViewModel.NewImage)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/");
        }

        public IActionResult Delete(int id)
        {
            var gameToDelete = this.gamesService.GetViewModelAsync<GameToEditViewModel>(id)
                .GetAwaiter()
                .GetResult();
            if (gameToDelete == null)
            {
                return this.Redirect("/");
            }

            return this.View(gameToDelete);
        }

        [HttpPost]
        public IActionResult Delete(GameToEditViewModel activityToEditViewModel)
        {
            var id = activityToEditViewModel.Id;
            this.gamesService.DeleteAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/");
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
