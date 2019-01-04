namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Messages;

    public interface IMessagesService
    {
        Task<int> CreateAsync(MessageInputModel inputModel);

        Task DeleteAsync(int id);

        Task MessageSeenAsync(int id);

        Task<MessageOutputModel[]> GetAllMessagesAsync(string username);
    }
}
