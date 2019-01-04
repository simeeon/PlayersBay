namespace PlayersBay.Services.Data
{
    using System;
    using System.Threading.Tasks;

    using CloudinaryDotNet;
    using PlayersBay.Data.Models.Enums;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Transfers;
    using System.Linq;
    using PlayersBay.Services.Mapping;

    public class TransfersService : ITransfersService
    {
        private readonly IRepository<Transfer> transfersRepository;
        private readonly IRepository<ApplicationUser> usersRepository;

        public TransfersService(
            IRepository<Transfer> transfersRepository,
            IRepository<ApplicationUser> usersRepository)
        {
            this.transfersRepository = transfersRepository;
            this.usersRepository = usersRepository;
        }

        public async Task<int> CreateDepositRequestAsync(TransferInputModel inputModel)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(u => u.UserName == inputModel.Username);

            var deposit = new Transfer
            {
                Amount = inputModel.Amount,
                Status = TransferStatus.Pending,
                Type = TransferType.Deposit,
                User = user,
            };

            this.transfersRepository.Add(deposit);
            await this.transfersRepository.SaveChangesAsync();

            return deposit.Id;
        }

        public async Task<int> CreateWithdrawalRequestAsync(TransferInputModel inputModel)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(u => u.UserName == inputModel.Username);

            var withdrawal = new Transfer
            {
                Amount = inputModel.Amount,
                Status = TransferStatus.Pending,
                Type = TransferType.Withdraw,
                User = user,
            };

            this.transfersRepository.Add(withdrawal);
            await this.transfersRepository.SaveChangesAsync();

            return withdrawal.Id;
        }

        public async Task<TransferViewModel[]> GetAllTransfersAsync(string username)
        {
            var receiver = this.usersRepository.All().FirstOrDefault(u => u.UserName == username);

            var transfers = await this.transfersRepository
                .All()
                .Where(u => u.User == receiver)
                .To<TransferViewModel>()
                .ToArrayAsync();

            return transfers;
        }
    }
}
