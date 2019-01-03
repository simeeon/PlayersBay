namespace PlayersBay.Services.Data.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class GamesCreateInputModel
    {
        [Required]
        [StringLength(Constants.Game.GameMaxLength, MinimumLength = Constants.Game.GameMinLength, ErrorMessage = Constants.Game.GameLengthError)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        public string ImageUrl { get; set; }
    }
}
