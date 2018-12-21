namespace PlayersBay.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PlayersBay.Common;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Web.ViewModels.Offers;

    public class OffersController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly IGamesService gamesService;
        private readonly IRepository<Offer> offerRepository;
        private readonly IRepository<Game> gameRepository;

        public OffersController(IOffersService offersService, IGamesService gamesService, IRepository<Offer> offerRepository, IRepository<Game> gameRepository)
        {
            this.offersService = offersService;
            this.gamesService = gamesService;
            this.offerRepository = offerRepository;
            this.gameRepository = gameRepository;
        }

        public IActionResult Index()
        {
            // var allGamesViewModel = this.offersService.GetAllActivitiesAsync()
            //    .GetAwaiter()
            //    .GetResult();

            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName + "," + GlobalConstants.UserRoleName)]
        public IActionResult Create()
        {
            this.ViewData["Games"] = this.SelectAllGames();
            return this.View();
        }

        [Authorize(Roles = GlobalConstants.AdministratorRoleName + "," + GlobalConstants.UserRoleName)]
        [HttpPost]
        public IActionResult Create(OfferCreateInputModel createInputModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(createInputModel);
            }

            var username = this.User.Identity.Name;

            var offerId = this.offersService.CreateAsync(
                username,
                createInputModel.GameId,
                createInputModel.Description,
                createInputModel.Duration,
                createInputModel.ImageUrl,
                createInputModel.MessageToBuyer,
                createInputModel.OfferType,
                createInputModel.Price,
                createInputModel.Title)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/");
        }

        public IActionResult Details(int id)
        {
            var detailViewModel = this.offersService.GetDetailsAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.View(detailViewModel);
        }

        public IActionResult All(int id)
        {
            var offers = this.offersService.GetAllOffersAsync(id)
                .GetAwaiter()
                .GetResult();

            this.ViewData["Game"] = this.gameRepository.All().FirstOrDefault(g => g.Id == id).Name;

            return this.View(offers);
        }

        private IEnumerable<SelectListItem> SelectAllGames()
        {
            return this.gamesService.GetAllGamesAsync()
                .GetAwaiter()
                .GetResult()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });
        }
    }
}
