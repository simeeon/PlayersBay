namespace PlayersBay.Services.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Messages;
    using PlayersBay.Services.Data.Models.Transactions;

    public class TransactionsService : ITransactionsService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly IRepository<Transaction> transactionsRepository;
        private readonly IRepository<Offer> offersRepository;
        private readonly UserManager<ApplicationUser> usersManager;
        private readonly IOffersService offersService;
        private readonly IMessagesService messagesService;
        private readonly IRepository<Message> messagesRepository;

        public TransactionsService(
            IRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> usersManager,
            IOffersService offersService,
            IRepository<Offer> offersRepository,
            IRepository<Transaction> transactionsRepository,
            IRepository<Message> messagesRepository,
            IMessagesService messagesService)
        {
            this.usersRepository = usersRepository;
            this.usersManager = usersManager;
            this.offersService = offersService;
            this.offersRepository = offersRepository;
            this.transactionsRepository = transactionsRepository;
            this.messagesRepository = messagesRepository;
            this.messagesService = messagesService;
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

                await this.messagesService.CreateAsync(new MessageInputModel
                {
                    Message = offer.MessageToBuyer,
                    ReceiverName = buyer.UserName,
                    SenderName = seller.UserName,
                });

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
