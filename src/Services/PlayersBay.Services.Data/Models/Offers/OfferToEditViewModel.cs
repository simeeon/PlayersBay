namespace PlayersBay.Services.Data.Models.Offers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Mapping;

    public class OfferToEditViewModel : IMapFrom<Offer>
    {
        public int Id { get; set; }

        public int GameId { get; set; }

        public Game Game { get; set; }

        public int Duration { get; set; }

        public string AuthorId { get; set; }

        [Display(Name = "Seller")]
        public ApplicationUser Author { get; set; }

        [Display(Name = "Offer Type")]
        public OfferType OfferType { get; set; }

        [Display(Name = "Current image")]
        public string ImageUrl { get; set; }

        [Display(Name = "New Image")]
        [DataType(DataType.Upload)]
        public IFormFile NewImage { get; set; }

        [Display(Name = "Offer ends")]
        public DateTime ExpiryDate { get; set; }

        public Status Status { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Display(Name = "Message To Buyer")]
        public string MessageToBuyer { get; set; }
    }
}
