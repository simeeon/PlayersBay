namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Transfers;

    public interface ITransfersService
    {
        Task<int> CreateDepositRequestAsync(string username, TransferInputModel inputModel);

        Task<int> CreateWithdrawalRequestAsync(string username, TransferInputModel inputModel);

        Task<TransferViewModel[]> GetAllTransfersAsync(string username);

        Task<TransferViewModel[]> GetAllDepositRequestsAsync();

        Task<TransferViewModel[]> GetAllWithdrawalRequestsAsync();

        Task ApproveTransferAsync(int id);

        Task DeclineTransferAsync(int id);
    }
}
