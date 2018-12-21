namespace PlayersBay.Services.Data.Models.Games
{
    using System.ComponentModel.DataAnnotations;

    using Microsoft.AspNetCore.Http;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Mapping;

    public class GameToEditViewModel : IMapFrom<Game>
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
