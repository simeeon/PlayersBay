namespace PlayersBay.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PlayersBay.Common;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Feedbacks;
    using PlayersBay.Services.Data.Models.Offers;
    using PlayersBay.Services.Data.Utilities;

    public class OffersController : BaseController
    {
        private readonly IOffersService offersService;
        private readonly IGamesService gamesService;
        private readonly IRepository<Feedback> feedbackRepository;
        private readonly IRepository<Game> gameRepository;

        public OffersController(
            IOffersService offersService,
            IGamesService gamesService,
            IRepository<Game> gameRepository,
            IRepository<Feedback> feedbackRepository)
        {
            this.offersService = offersService;
            this.gamesService = gamesService;
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
            var sellerUsername = this.User.Identity.Name;

            if (!this.ModelState.IsValid)
            {
                return this.View(createInputModel);
            }

            var offerId = this.offersService.CreateAsync(sellerUsername, createInputModel).GetAwaiter().GetResult();

            return this.RedirectToAction("ActiveOffers", "Offers").WithInfo(DataConstants.NotificationMessages.Info, string.Format(DataConstants.NotificationMessages.OfferCreated, offerId));
        }

        public IActionResult Details(int id)
        {
            var detailViewModel = this.offersService.GetViewModelAsync<OfferDetailsViewModel>(id).GetAwaiter().GetResult();

            this.ViewData["Game"] = this.gameRepository.All().FirstOrDefault(g => g.Id == detailViewModel.GameId).Name;

            return this.View(detailViewModel);
        }

        public IActionResult All(int id)
        {
            var offers = this.offersService.GetAllOffersAsync(id).GetAwaiter().GetResult();

            this.ViewData["Game"] = this.gameRepository.All().FirstOrDefault(g => g.Id == id).Name;

            return this.View(offers);
        }

        [Authorize]
        public IActionResult ActiveOffers()
        {
            var username = this.User.Identity.Name;

            var offers = this.offersService.GetMyActiveOffersAsync(username).GetAwaiter().GetResult();

            return this.View(offers);
        }

        [Authorize]
        public IActionResult SoldOffers()
        {
            this.ViewData["Feedbacks"] = this.feedbackRepository.All().Select(x => new FeedbacksViewModel
            {
                OfferId = x.OfferId,
                Content = x.Content,
                FeedbackRating = x.FeedbackRating,
                HasFeedback = x.HasFeedback,
            }).ToArray();

            var username = this.User.Identity.Name;

            var offers = this.offersService.GetMySoldOffersAsync(username).GetAwaiter().GetResult();

            return this.View(offers);
        }

        [Authorize]
        public IActionResult BoughtOffers()
        {
            this.ViewData["Feedbacks"] = this.feedbackRepository.All().Select(x => new FeedbacksViewModel
            {
                  OfferId = x.OfferId,
                  Content = x.Content,
                  FeedbackRating = x.FeedbackRating,
                  HasFeedback = x.HasFeedback,
            }).ToArray();

            var username = this.User.Identity.Name;

            var offers = this.offersService.GetMyBoughtOffersAsync(username).GetAwaiter().GetResult();

            return this.View(offers);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var offerToEdit = this.offersService.GetViewModelAsync<OfferToEditViewModel>(id).GetAwaiter().GetResult();

            if ((offerToEdit.Seller == null || offerToEdit.Seller.UserName != this.User.Identity.Name) &&
                this.User.IsInRole(GlobalConstants.UserRoleName))
            {
                return this.View("_AccessDenied");
            }

            this.ViewData["Games"] = this.SelectAllGames();
            return this.View(offerToEdit);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Edit(OfferToEditViewModel offerToEditViewModel)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(offerToEditViewModel);
            }

            var offerId = offerToEditViewModel.Id;
            this.offersService.EditAsync(offerToEditViewModel).GetAwaiter().GetResult();

            return this.RedirectToAction("Details", "Offers", new { id = offerId }).WithInfo(DataConstants.NotificationMessages.Info, string.Format(DataConstants.NotificationMessages.OfferEdited, offerId));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var offerToDelete = this.offersService.GetViewModelAsync<OfferToEditViewModel>(id)
                .GetAwaiter()
                .GetResult();

            if ((offerToDelete.Seller == null || offerToDelete.Seller.UserName != this.User.Identity.Name) &&
                this.User.IsInRole(GlobalConstants.UserRoleName))
            {
                return this.View("_AccessDenied");
            }

            this.ViewData["Game"] = this.gameRepository.All().FirstOrDefault(g => g.Id == offerToDelete.GameId).Name;

            return this.View(offerToDelete);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(OfferToEditViewModel offerToEditViewModel)
        {
            var id = offerToEditViewModel.Id;
            this.offersService.DeleteAsync(id).GetAwaiter().GetResult();

            return this.RedirectToAction("ActiveOffers", "Offers").WithInfo(DataConstants.NotificationMessages.Info, string.Format(DataConstants.NotificationMessages.OfferDeleted, id));
        }

        private IEnumerable<SelectListItem> SelectAllGames()
        {
            return this.gamesService.GetAllGamesAsync().GetAwaiter().GetResult()
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.Name,
                });
        }
    }
}
