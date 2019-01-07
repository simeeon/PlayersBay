﻿namespace PlayersBay.Services.Data.Contracts
{
    using System.Threading.Tasks;

    using PlayersBay.Services.Data.Models.Messages;

    public interface IMessagesService
    {
        Task<int> CreateAsync(string senderUsername, MessageInputModel inputModel);

        Task DeleteAsync(int id);

        Task MessageSeenAsync(int id);

        Task<MessageOutputModel[]> GetAllMessagesAsync(string username);

        Task<MessageOutputModel[]> GetOutboxAsync(string username);
    }
}
