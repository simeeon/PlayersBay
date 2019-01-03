namespace PlayersBay.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
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

        public async Task<int> CreateAsync(GamesCreateInputModel inputModel)
        {
            var game = new Game
            {
                Name = inputModel.Name,
            };

            if (inputModel.Image != null)
            {
                game.ImageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, inputModel.Image, inputModel.Name);
            }
            else
            {
                game.ImageUrl = inputModel.ImageUrl;
            }

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

        public async Task EditAsync(GameToEditViewModel editViewModel)
        {
            var game = await this.gamesRepository.All().FirstOrDefaultAsync(a => a.Id == editViewModel.Id);

            if (editViewModel.NewImage != null)
            {
                var newImageUrl = await ApplicationCloudinary.UploadImage(this.cloudinary, editViewModel.NewImage, editViewModel.Name);
                game.ImageUrl = newImageUrl;
            }

            game.Name = editViewModel.Name;

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
