using PlayersBay.Data.Models;
using PlayersBay.Data.Models.Enums;
using PlayersBay.Services.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlayersBay.Services.Data.Models.Offers
{
    public class OfferViewModel : IMapFrom<Offer>
    {
        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public OfferType OfferType { get; set; }

        public string ImageUrl { get; set; }

        public DateTime Duration { get; set; }

        public Status Status { get; set; }

        public decimal Price { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string MessageToBuyer { get; set; }

        // public int FeedbackId { get; set; }
        // 
        // public virtual Feedback Feedback { get; set; }
    }
}
