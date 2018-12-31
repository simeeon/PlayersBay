namespace PlayersBay.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Transactions;
    using PlayersBay.Services.Mapping;

    public class TransactionsService : ITransactionsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Transaction> transactionsRepository;
        private readonly IRepository<Offer> offersRepository;
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IOffersService offersService;

        public TransactionsService(
            IRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> usersManager,
            IOffersService offersService,
            IRepository<Offer> offersRepository,
            IRepository<Transaction> transactionsRepository)
        {
            this.usersRepository = usersRepository;
            this.usersManager = usersManager;
            this.offersService = offersService;
            this.offersRepository = offersRepository;
            this.transactionsRepository = transactionsRepository;
        }

        public async Task<int> CreateAsync(TransactionInputModel inputModel)
        {
            var buyer = this.usersManager.FindByNameAsync(inputModel.BuyerName).GetAwaiter().GetResult();
            var seller = this.usersManager.FindByNameAsync(inputModel.SellerName).GetAwaiter().GetResult();
            var offer = this.offersRepository.GetByIdAsync(inputModel.OfferId).GetAwaiter().GetResult();

            if (buyer.Balance >= offer.Price)
            {
                buyer.Balance -= offer.Price;
                seller.Balance += offer.Price;

                offer.Status = PlayersBay.Data.Models.Enums.Status.Completed;

                this.usersRepository.Update(buyer);
                this.usersRepository.Update(seller);
                this.offersRepository.Update(offer);
                var transaction = new Transaction
                {
                    BuyerId = buyer.Id,
                    SellerId = seller.Id,
                    Offer = offer,
                };
                this.transactionsRepository.Add(transaction);
                await this.offersRepository.SaveChangesAsync();
                await this.usersRepository.SaveChangesAsync();
                await this.transactionsRepository.SaveChangesAsync();

                return transaction.Id;
            }

            return 0;
        }

        public async Task TopUpAsync(TopUpInputModel inputModel)
        {
            var user = this.usersManager.FindByNameAsync(inputModel.Username).GetAwaiter().GetResult();

            user.Balance += inputModel.Amount;
            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
