namespace PlayersBay.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models;
    using PlayersBay.Services.Data.Models.Offers;
    using PlayersBay.Services.Data.Utilities;
    using PlayersBay.Services.Mapping;

    public class OffersService : IOffersService
    {
        private readonly IRepository<Offer> offersRepository;
        private readonly IRepository<Transaction> transactionRepository;
        private readonly Cloudinary cloudinary;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> usersManager;

        public OffersService(
            IRepository<Offer> offersRepository,
            Cloudinary cloudinary,
            IRepository<ApplicationUser> usersRepository,
            IRepository<Transaction> transactionRepository,
            UserManager<ApplicationUser> usersManager)
        {
            this.offersRepository = offersRepository;
            this.cloudinary = cloudinary;
            this.usersRepository = usersRepository;
            this.transactionRepository = transactionRepository;
            this.usersManager = usersManager;
        }

        public async Task<int> CreateAsync(OfferCreateInputModel inputModel)
        {
            var seller = this.usersManager.FindByNameAsync(inputModel.Author).GetAwaiter().GetResult();

            var imageUrl = string.Empty;
            if (inputModel.ImageUrl != null)
            {
                imageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, inputModel.ImageUrl, inputModel.Title);
            }

            var offer = new Offer
            {
                GameId = inputModel.GameId,
                Author = seller,
                Description = inputModel.Description,
                ExpiryDate = DateTime.UtcNow.AddDays(inputModel.Duration),
                ImageUrl = imageUrl,
                MessageToBuyer = inputModel.MessageToBuyer,
                OfferType = inputModel.OfferType,
                Price = inputModel.Price,
                Title = inputModel.Title,
                Status = Status.Active,
            };

            this.offersRepository.Add(offer);
            await this.offersRepository.SaveChangesAsync();

            return offer.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var offer = this.offersRepository.All().FirstOrDefault(d => d.Id == id);
            offer.IsDeleted = true;

            this.offersRepository.Update(offer);
            await this.offersRepository.SaveChangesAsync();
        }

        public async Task EditAsync(OfferToEditViewModel offerToEdit)
        {
            var offer = this.offersRepository.All().FirstOrDefault(d => d.Id == offerToEdit.Id);

            if (offerToEdit.NewImage != null)
            {
                var newImageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, offerToEdit.NewImage, offerToEdit.Title);
                offer.ImageUrl = newImageUrl;
            }

            offer.Price = offerToEdit.Price;
            offer.Title = offerToEdit.Title;
            offer.ExpiryDate = DateTime.UtcNow.AddDays(offerToEdit.Duration);
            offer.OfferType = offerToEdit.OfferType;
            offer.MessageToBuyer = offerToEdit.MessageToBuyer;
            offer.Description = offerToEdit.Description;

            this.offersRepository.Update(offer);
            await this.offersRepository.SaveChangesAsync();
        }

        public async Task<OfferViewModel[]> GetAllOffersAsync(int id)
        {
            var offers = await this.offersRepository
                .All()
                .Where(o => o.GameId == id && o.Status == Status.Active)
                .To<OfferViewModel>()
                .ToArrayAsync();

            return offers;
        }

        public async Task<OfferViewModel[]> GetMyActiveOffersAsync(string username)
        {
            var offers = await this.offersRepository
                .All()
                .Where(o => o.Author.UserName == username && o.Status == Status.Active)
                .To<OfferViewModel>()
                .ToArrayAsync();

            return offers;
        }

        public async Task<OfferViewModel[]> GetMySoldOffersAsync(string username)
        {
            var offers = await this.offersRepository
                .All()
                .Where(o => o.Author.UserName == username && o.Status == Status.Completed)
                .To<OfferViewModel>()
                .ToArrayAsync();

            return offers;
        }

        public async Task<OfferViewModel[]> GetMyBoughtOffersAsync(string username)
        {
            var offerIds = this.transactionRepository
                .All()
                .Where(b => b.Buyer.UserName == username)
                .Select(o => o.Id)
                .ToList();

            var offers = await this.offersRepository
                .All()
                .Where(o => offerIds.Contains(o.Id) && o.Status == Status.Completed)
                .To<OfferViewModel>()
                .ToArrayAsync();

            return offers;
        }

        public async Task<TViewModel> GetViewModelAsync<TViewModel>(int id)
        {
            var offer = await this.offersRepository
                .All()
                .Where(a => a.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            if (offer == null)
            {
                throw new NullReferenceException(string.Format(Constants.NullReferenceOfferId, id));
            }

            return offer;
        }
    }
}
