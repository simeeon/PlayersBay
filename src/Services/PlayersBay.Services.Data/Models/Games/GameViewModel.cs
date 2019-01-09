namespace PlayersBay.Services.Data.Models.Games
{
    using System.Collections.Generic;

    using PlayersBay.Services.Data.Models.Offers;

    public class GameViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public ICollection<OfferViewModel> Offers { get; set; }
    }
}
