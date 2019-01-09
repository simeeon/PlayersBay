namespace PlayersBay.Services.Data.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using PlayersBay.Services.Data.Utilities;

    public class GamesCreateInputModel
    {
        [Required]
        [StringLength(DataConstants.Game.GameMaxLength, MinimumLength = DataConstants.Game.GameMinLength, ErrorMessage = DataConstants.Game.GameLengthError)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }
    }
}
