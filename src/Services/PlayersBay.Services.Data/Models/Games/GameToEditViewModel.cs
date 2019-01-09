namespace PlayersBay.Services.Data.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;

    public class GameToEditViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Current image")]
        public string ImageUrl { get; set; }

        [Display(Name = "New Image")]
        [DataType(DataType.Upload)]
        public IFormFile NewImage { get; set; }
    }
}
