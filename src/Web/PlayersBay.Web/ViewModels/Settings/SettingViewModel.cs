namespace PlayersBay.Web.ViewModels.Settings
{
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Mapping;

    public class SettingViewModel : IMapFrom<Setting>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Value { get; set; }
    }
}
