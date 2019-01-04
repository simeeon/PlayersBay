namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Transfers;

    public interface ITransfersService
    {
        Task<int> CreateDepositRequestAsync(TransferInputModel inputModel);

        Task<int> CreateWithdrawalRequestAsync(TransferInputModel inputModel);

        Task<TransferViewModel[]> GetAllTransfersAsync(string username);
    }
}
