namespace PlayersBay.Services.Data.Models.Games
{
    using System.Collections.Generic;

    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Models.Offers;
    using PlayersBay.Services.Mapping;

    public class GameViewModel : IMapFrom<Game>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<OfferViewModel> Offers { get; set; }
    }
}
