namespace PlayersBay.Web.ViewModels.Offers
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using PlayersBay.Data.Models.Enums;

    public class OfferCreateInputModel
    {
        [Required]
        [Display(Name = "Game")]
        public int GameId { get; set; }

        [Required]
        [Display(Name = "Offer Type")]
        public OfferType OfferType { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageUrl { get; set; }

        [Required]
        public string Duration { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(1, 50000)]
        public decimal Price { get; set; }

        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Content must be between 5 and 500 symbols")]
        public string Description { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(500, MinimumLength = 5, ErrorMessage = "Content must be between 5 and 500 symbols")]
        public string MessageToBuyer { get; set; }
    }
}
