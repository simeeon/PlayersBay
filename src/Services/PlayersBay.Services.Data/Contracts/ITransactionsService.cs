namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Transactions;

    public interface ITransactionsService
    {
        Task<int> CreateAsync(TransactionInputModel inputModel);

        Task TopUpAsync(TopUpInputModel inputModel);
    }
}
