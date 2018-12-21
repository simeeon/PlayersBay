namespace PlayersBay.Services.Data.Models.Offers
{

    using PlayersBay.Data.Models;
    using PlayersBay.Services.Mapping;
    using System.ComponentModel.DataAnnotations;

    public class OfferDetailsViewModel : IMapFrom<Offer>
    {
        public int Id { get; set; }

        [Display(Name = "Game")]
        public int GameId { get; set; }

        [Display(Name = "Offer Type")]
        public string OfferType { get; set; }

        [Display(Name = "Image")]
        public string ImageUrl { get; set; }

        public string Duration { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        [StringLength(500, MinimumLength = 5, ErrorMessage = "Content must be between 5 and 500 symbols")]
        public string Description { get; set; }
    }
}
