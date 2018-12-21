namespace PlayersBay.Services.Data.Models.Offers
{
    using System;

    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Mapping;

    public class OfferViewModel : IMapFrom<Offer>
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public OfferType OfferType { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Duration { get; set; }

        public Status Status { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string MessageToBuyer { get; set; }
    }
}
