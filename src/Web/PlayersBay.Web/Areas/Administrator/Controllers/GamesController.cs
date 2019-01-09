namespace PlayersBay.Web.Areas.Administrator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Games;
    using PlayersBay.Services.Data.Utilities;

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

            var gameId = this.gamesService.CreateAsync(gameCreateInputModel).GetAwaiter().GetResult();

            return this.Redirect("/").WithSuccess(DataConstants.NotificationMessages.Success, string.Format(DataConstants.NotificationMessages.GameCreated, gameCreateInputModel.Name));
        }

        public IActionResult Edit(int id)
        {
            this.ViewData["id"] = id;

            var gameToEdit = this.gamesService.GetViewModelAsync<GameToEditViewModel>(id).GetAwaiter().GetResult();

            return this.View(gameToEdit);
        }

        [HttpPost]
        public IActionResult Edit(GameToEditViewModel gameToEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(gameToEditViewModel);
            }

            var id = gameToEditViewModel.Id;
            this.gamesService.EditAsync(gameToEditViewModel).GetAwaiter().GetResult();

            return this.Redirect("/").WithSuccess(DataConstants.NotificationMessages.Success, string.Format(DataConstants.NotificationMessages.GameEdited, gameToEditViewModel.Name));
        }

        public IActionResult Delete(int id)
        {
            var gameToDelete = this.gamesService.GetViewModelAsync<GameToEditViewModel>(id)
                .GetAwaiter()
                .GetResult();

            return this.View(gameToDelete);
        }

        [HttpPost]
        public IActionResult Delete(GameToEditViewModel gameToEditViewModel)
        {
            var id = gameToEditViewModel.Id;
            this.gamesService.DeleteAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/").WithInfo(DataConstants.NotificationMessages.Info, string.Format(DataConstants.NotificationMessages.GameDeleted, gameToEditViewModel.Name));
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
