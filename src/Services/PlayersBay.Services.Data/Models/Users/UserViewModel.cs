namespace PlayersBay.Services.Data.Models.Users
{
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Mapping;

    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Username { get; set; }
    }
}
