namespace PlayersBay.Web.Areas.Administrator.InputModels.Games
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class GamesCreateInputModel
    {
        [Required]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Game name must be between 3 and 30 symbols")]
        public string Name { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageUrl { get; set; }
    }
}
