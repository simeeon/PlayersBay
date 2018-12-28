namespace PlayersBay.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Games;
    using PlayersBay.Services.Data.Utilities;
    using PlayersBay.Services.Mapping;

    public class GamesService : IGamesService
    {
        private readonly IRepository<Game> gamesRepository;
        private readonly Cloudinary cloudinary;

        public GamesService(IRepository<Game> gamesRepository, Cloudinary cloudinary)
        {
            this.gamesRepository = gamesRepository;
            this.cloudinary = cloudinary;
        }

        public async Task<int> CreateAsync(params object[] parameters)
        {
            var name = parameters[0].ToString();

            var image = new object();
            var imageUrl = string.Empty;

            if (parameters[1] is string)
            {
                imageUrl = parameters[1].ToString();
            }
            else
            {
                image = parameters[1] as IFormFile;

                imageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, (IFormFile)image, name);
            }

            var game = new Game
            {
                Name = name,
                ImageUrl = imageUrl,
            };

            this.gamesRepository.Add(game);
            await this.gamesRepository.SaveChangesAsync();

            return game.Id;
        }

        public async Task DeleteAsync(int id)
        {
            var game = this.gamesRepository.All().FirstOrDefault(d => d.Id == id);
            game.IsDeleted = true;

            this.gamesRepository.Update(game);
            await this.gamesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(int id, params object[] parameters)
        {
            var name = parameters[0].ToString();
            var newImage = parameters[1] as IFormFile;

            var game = await this.gamesRepository.All().FirstOrDefaultAsync(a => a.Id == id);

            if (newImage != null)
            {
                var newImageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, newImage, name);
                game.ImageUrl = newImageUrl;
            }

            game.Name = name;

            this.gamesRepository.Update(game);
            await this.gamesRepository.SaveChangesAsync();
        }

        public async Task<GameViewModel[]> GetAllGamesAsync()
        {
            var games = await this.gamesRepository
                .All()
                .To<GameViewModel>()
                .ToArrayAsync();

            return games;
        }

        public async Task<TViewModel> GetViewModelAsync<TViewModel>(int id)
        {
            var game = await this.gamesRepository
                .All()
                .Where(a => a.Id == id)
                .To<TViewModel>()
                .FirstOrDefaultAsync();

            return game;
        }
    }
}
