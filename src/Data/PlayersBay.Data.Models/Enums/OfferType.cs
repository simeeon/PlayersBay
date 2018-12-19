namespace PlayersBay.Data.Models.Enums
{
    using System.ComponentModel.DataAnnotations;

    public enum OfferType
    {
        Account = 0,
        Items = 1,
        [Display(Name = "CD Key")]
        CDKey = 2,
        Service = 3,
    }
}
