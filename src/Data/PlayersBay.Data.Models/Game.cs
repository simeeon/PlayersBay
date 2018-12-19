namespace PlayersBay.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using PlayersBay.Data.Common.Models;

    public class Game : BaseDeletableModel<int>
    {
        public Game()
        {
            this.Offers = new HashSet<Offer>();
        }

        public string Name { get; set; }

        public virtual ICollection<Offer> Offers { get; set; }
    }
}
