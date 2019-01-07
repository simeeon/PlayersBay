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

        [Display(Name = "Game")]
        public int GameId { get; set; }

        public Game Game { get; set; }

        [Required]
        public int Duration { get; set; }

        public string SellerId { get; set; }

        public ApplicationUser Seller { get; set; }

        [Required]
        [Display(Name = "Offer Type")]
        public OfferType OfferType { get; set; }

        [Display(Name = "Current image")]
        public string ImageUrl { get; set; }

        [Display(Name = "New Image")]
        [DataType(DataType.Upload)]
        public IFormFile NewImage { get; set; }

        [Required]
        [Display(Name = "Offer ends")]
        public DateTime ExpiryDate { get; set; }

        public OfferStatus Status { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(Constants.Offer.MinPrice, Constants.Offer.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(Constants.Offer.TitleMaxLength, MinimumLength = Constants.Offer.TitleMinLength, ErrorMessage = Constants.Offer.TitleLengthError)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(Constants.Offer.DescriptionMaxLength, MinimumLength = Constants.Offer.DescriptionMinLength, ErrorMessage = Constants.Offer.DescriptionLengthError)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(Constants.Offer.MessageToBuyerMaxLength, MinimumLength = Constants.Offer.MessageToBuyerMinLength, ErrorMessage = Constants.Offer.MessageToBuyerLengthError)]
        [Display(Name = "Message To Buyer")]
        public string MessageToBuyer { get; set; }
    }
}
