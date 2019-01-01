namespace PlayersBay.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Offers;
    using PlayersBay.Web.ViewModels.Feedbacks;
    using PlayersBay.Web.ViewModels.Offers;

    public class OffersController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly IGamesService gamesService;
        private readonly IRepository<Offer> offerRepository;
        private readonly IRepository<Feedback> feedbackRepository;
        private readonly IRepository<Game> gameRepository;

        public OffersController(
            IOffersService offersService,
            IGamesService gamesService,
            IRepository<Offer> offerRepository,
            IRepository<Game> gameRepository,
            IRepository<Feedback> feedbackRepository)
        {
            this.offersService = offersService;
            this.gamesService = gamesService;
            this.offerRepository = offerRepository;
            this.gameRepository = gameRepository;
            this.feedbackRepository = feedbackRepository;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize]
        public IActionResult Create()
        {
            this.ViewData["Games"] = this.SelectAllGames();
            return this.View();
        }

        [Authorize]
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

            return this.RedirectToAction("ActiveOffers", "Offers", new { username = this.User.Identity.Name });
        }

        public IActionResult Details(int id)
        {
            var detailViewModel = this.offersService.GetDetailsAsync(id)
                .GetAwaiter()
                .GetResult();

            this.ViewData["Game"] = this.gameRepository.All().FirstOrDefault(g => g.Id == detailViewModel.GameId).Name;

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

        [Authorize]
        public IActionResult ActiveOffers(string username)
        {
            var offers = this.offersService.GetMyActiveOffersAsync(username)
                .GetAwaiter()
                .GetResult();

            return this.View(offers);
        }

        [Authorize]
        public IActionResult SoldOffers(string username)
        {
            this.ViewData["Feedbacks"] = this.feedbackRepository.All().Select(x => new FeedbacksViewModel
            {
                OfferId = x.OfferId,
                Content = x.Content,
                FeedbackRating = x.FeedbackRating,
            }).ToArray();

            var offers = this.offersService.GetMySoldOffersAsync(username)
                .GetAwaiter()
                .GetResult();

            return this.View(offers);
        }

        [Authorize]
        public IActionResult BoughtOffers(string username)
        {
            this.ViewData["Feedbacks"] = this.feedbackRepository.All().Select(x => new FeedbacksViewModel
            {
                  OfferId = x.OfferId,
                  Content = x.Content,
                  FeedbackRating = x.FeedbackRating,
            }).ToArray();

            var offers = this.offersService.GetMyBoughtOffersAsync(username)
                .GetAwaiter()
                .GetResult();

            return this.View(offers);
        }

        public IActionResult Edit(int id)
        {
            var offerToEdit = this.offersService.GetViewModelAsync<OfferToEditViewModel>(id)
                .GetAwaiter()
                .GetResult();

            if (offerToEdit == null)
            {
                return this.Redirect("/");
            }

            this.ViewData["Games"] = this.SelectAllGames();
            return this.View(offerToEdit);
        }

        [HttpPost]
        public IActionResult Edit(OfferToEditViewModel offerToEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(offerToEditViewModel);
            }

            if (offerToEditViewModel.NewImage != null)
            {
                var fileType = offerToEditViewModel.NewImage.ContentType.Split('/')[1];
                if (!this.IsImageTypeValid(fileType))
                {
                    return this.View(offerToEditViewModel);
                }
            }

            var id = offerToEditViewModel.Id;
            this.offersService.EditAsync(offerToEditViewModel)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("Details", "Offers", new { id }).WithInfo("Success!", $"Offer #{id} edited");
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var offerToDelete = this.offersService.GetViewModelAsync<OfferToEditViewModel>(id)
                .GetAwaiter()
                .GetResult();
            if (offerToDelete == null)
            {
                return this.Redirect("/");
            }

            this.ViewData["Game"] = this.gameRepository.All().FirstOrDefault(g => g.Id == offerToDelete.GameId).Name;

            return this.View(offerToDelete);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(OfferToEditViewModel offerToEditViewModel)
        {
            var id = offerToEditViewModel.Id;
            this.offersService.DeleteAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("ActiveOffers", "Offers", new { username = this.User.Identity.Name });
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
