﻿namespace PlayersBay.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Deals;
    using PlayersBay.Services.Data.Models.Messages;
    using PlayersBay.Services.Data.Utilities;

    public class DealsService : IDealsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Deal> dealsRepository;
        private readonly IRepository<Offer> offersRepository;
        private readonly IMessagesService messagesService;

        public DealsService(
            IRepository<ApplicationUser> usersRepository,
            IRepository<Offer> offersRepository,
            IRepository<Deal> transactionsRepository,
            IMessagesService messagesService)
        {
            this.offersRepository = offersRepository;
            this.dealsRepository = transactionsRepository;
            this.messagesService = messagesService;
            this.usersRepository = usersRepository;
        }

        public async Task CreateAsync(string buyerUsername, DealInputModel inputModel)
        {
            var buyer = await this.usersRepository.All().FirstOrDefaultAsync(u => u.UserName == buyerUsername);
            var seller = await this.usersRepository.All().FirstOrDefaultAsync(u => u.UserName == inputModel.SellerName);
            var offer = this.offersRepository.GetByIdAsync(inputModel.OfferId).GetAwaiter().GetResult();

            if (buyer.Balance >= offer.Price)
            {
                buyer.Balance -= offer.Price;
                seller.Balance += offer.Price;

                offer.Status = PlayersBay.Data.Models.Enums.OfferStatus.Completed;

                this.usersRepository.Update(buyer);
                this.usersRepository.Update(seller);
                this.offersRepository.Update(offer);

                var deal = new Deal
                {
                    BuyerId = buyer.Id,
                    SellerId = seller.Id,
                    Offer = offer,
                };

                await this.messagesService.CreateAsync(seller.UserName, new MessageInputModel
                {
                    Message = offer.MessageToBuyer,
                    ReceiverName = buyer.UserName,
                });

                this.dealsRepository.Add(deal);
                await this.offersRepository.SaveChangesAsync();
                await this.usersRepository.SaveChangesAsync();
                await this.dealsRepository.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException(string.Format(DataConstants.InsufficientFundsError, offer.Price, buyer.Balance));
            }
        }
    }
}