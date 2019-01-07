namespace PlayersBay.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Models.Enums;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Transfers;
    using PlayersBay.Services.Data.Utilities;
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

        public async Task<int> CreateDepositRequestAsync(string username, TransferInputModel inputModel)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(u => u.UserName == username);

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

        public async Task<int> CreateWithdrawalRequestAsync(string username, TransferInputModel inputModel)
        {
            var user = await this.usersRepository.All().FirstOrDefaultAsync(u => u.UserName == username);

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

        public async Task<TransferViewModel[]> GetAllDepositRequestsAsync()
        {
            var depositRequests = await this.transfersRepository
                .All()
                .Where(u => u.Type == TransferType.Deposit && u.Status == TransferStatus.Pending)
                .To<TransferViewModel>()
                .ToArrayAsync();

            return depositRequests;
        }

        public async Task<TransferViewModel[]> GetAllWithdrawalRequestsAsync()
        {
            var withdrawalRequests = await this.transfersRepository
                .All()
                .Where(u => u.Type == TransferType.Withdraw && u.Status == TransferStatus.Pending)
                .To<TransferViewModel>()
                .ToArrayAsync();

            return withdrawalRequests;
        }

        public async Task ApproveTransferAsync(int id)
        {
            var transfer = this.transfersRepository.All().FirstOrDefault(t => t.Id == id);

            var user = this.usersRepository.All().FirstOrDefault(u => u.Id == transfer.UserId);

            if (transfer.Type == TransferType.Deposit)
            {
                user.Balance += transfer.Amount;
            }
            else
            {
                if (user.Balance >= transfer.Amount)
                {
                    user.Balance -= transfer.Amount;
                }
                else
                {
                    throw new InvalidOperationException(string.Format(DataConstants.InvalidWithdrawalAmount, transfer.Amount, user.UserName, user.Balance));
                }
            }

            this.usersRepository.Update(user);
            await this.usersRepository.SaveChangesAsync();

            transfer.Status = TransferStatus.Approved;

            this.transfersRepository.Update(transfer);
            await this.usersRepository.SaveChangesAsync();
        }

        public async Task DeclineTransferAsync(int id)
        {
            var transfer = this.transfersRepository.All().FirstOrDefault(u => u.Id == id);

            transfer.Status = TransferStatus.Declined;

            this.transfersRepository.Update(transfer);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
