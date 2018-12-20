namespace PlayersBay.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;
    using CloudinaryDotNet;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
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
            var image = parameters[1] as IFormFile;

            var imageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, image, name);

            var game = new Game
            {
                Name = name,
                ImageUrl = imageUrl,
            };

            this.gamesRepository.Add(game);
            await this.gamesRepository.SaveChangesAsync();

            return game.Id;
        }

        public async Task<GameViewModel[]> GetAllGamesAsync()
        {
            var games = await this.gamesRepository
                .All()
                .To<GameViewModel>()
                .ToArrayAsync();

            return games;
        }
    }
}
