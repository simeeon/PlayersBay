namespace PlayersBay.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Games;
    using PlayersBay.Services.Data.Models.Offers;
    using PlayersBay.Services.Data.Utilities;
    using PlayersBay.Services.Mapping;

    // TODO: Extract interface

    public class OffersService : IOffersService
    {
        private readonly IRepository<Offer> offersRepository;
        private readonly Cloudinary cloudinary;
        private readonly IRepository<ApplicationUser> usersRepository;

        public OffersService(IRepository<Offer> offersRepository, Cloudinary cloudinary, IRepository<ApplicationUser> usersRepository)
        {
            this.offersRepository = offersRepository;
            this.cloudinary = cloudinary;
            this.usersRepository = usersRepository;
        }

        public async Task<int> CreateAsync(string username, params object[] parameters)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(u => u.UserName == username);

            var gameId = int.Parse(parameters[0].ToString());
            var description = parameters[1].ToString();
            var duration = double.Parse(parameters[2].ToString());

            var image = new object();
            var imageUrl = string.Empty;

            if (parameters[3] != null)
            {
                image = parameters[3] as IFormFile;
                imageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, (IFormFile)image, parameters[1].ToString());
            }

            var messageToBuyer = parameters[4].ToString();
            var offerType = parameters[5].ToString();
            Enum.TryParse(offerType, true, out OfferType typeEnum);
            var price = decimal.Parse(parameters[6].ToString());
            var title = parameters[7].ToString();

            var offer = new Offer
            {
                GameId = gameId,
                Author = user,
                Description = description,
                ExpiryDate = DateTime.UtcNow.AddDays(duration),
                ImageUrl = imageUrl,
                MessageToBuyer = messageToBuyer,
                OfferType = typeEnum,
                Price = price,
                Title = title,
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

            if (offer == null)
            {
                throw new NullReferenceException();
            }

            if (offerToEdit.NewImage != null)
            {
                var newImageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, offerToEdit.NewImage, offerToEdit.Description);
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
                .Where(o => o.GameId == id)
                .To<OfferViewModel>()
                .ToArrayAsync();

            return offers;
        }

        public async Task<OfferDetailsViewModel> GetDetailsAsync(int id)
        {
            var offerDetailsViewModel = await this.offersRepository
                .All()
                .To<OfferDetailsViewModel>()
                .FirstOrDefaultAsync(r => r.Id == id);

            return offerDetailsViewModel;
        }

        public async Task<TViewModel> GetViewModelAsync<TViewModel>(int id)
        {
            var game = await this.offersRepository
                .All()
                .Where(a => a.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return game;
        }
    }
}
