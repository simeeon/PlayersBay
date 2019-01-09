namespace PlayersBay.Services.Data.Models.Offers
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Utilities;

    public class OfferToEditViewModel
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
        [Range(DataConstants.Offer.MinPrice, DataConstants.Offer.MaxPrice)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(DataConstants.Offer.TitleMaxLength, MinimumLength = DataConstants.Offer.TitleMinLength, ErrorMessage = DataConstants.Offer.TitleLengthError)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(DataConstants.Offer.DescriptionMaxLength, MinimumLength = DataConstants.Offer.DescriptionMinLength, ErrorMessage = DataConstants.Offer.DescriptionLengthError)]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(DataConstants.Offer.MessageToBuyerMaxLength, MinimumLength = DataConstants.Offer.MessageToBuyerMinLength, ErrorMessage = DataConstants.Offer.MessageToBuyerLengthError)]
        [Display(Name = "Message To Buyer")]
        public string MessageToBuyer { get; set; }
    }
}
