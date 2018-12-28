﻿namespace PlayersBay.Services.Data
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Messages;
    using PlayersBay.Services.Mapping;

    public class MessagesService : IMessagesService
    {
        private readonly IRepository<Message> messageRepository;
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> usersManager;

        public MessagesService(
            IRepository<ApplicationUser> usersRepository,
            IRepository<Message> messageRepository,
            UserManager<ApplicationUser> usersManager)
        {
            this.messageRepository = messageRepository;
            this.usersRepository = usersRepository;
            this.usersManager = usersManager;
        }

        public async Task CreateAsync(MessageInputModel inputModel)
        {
            var sender = this.usersManager.FindByNameAsync(inputModel.SenderName).GetAwaiter().GetResult();
            var receiver = this.usersManager.FindByNameAsync(inputModel.ReceiverName).GetAwaiter().GetResult();

            var message = new Message
            {
                IsRead = false,
                Text = inputModel.Message,
                ReceiverId = receiver.Id,
                SenderId = sender.Id,
            };

            this.messageRepository.Add(message);
            await this.messageRepository.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var message = this.messageRepository.All().FirstOrDefault(d => d.Id == id);
            message.IsDeleted = true;

            this.messageRepository.Update(message);
            await this.messageRepository.SaveChangesAsync();
        }

        public async Task MessageSeenAsync(int id)
        {
            var message = this.messageRepository.All().FirstOrDefault(d => d.Id == id);
            message.IsRead = true;

            this.messageRepository.Update(message);
            await this.messageRepository.SaveChangesAsync();
        }

        public async Task<MessageOutputModel[]> GetAllMessagesAsync(string username)
        {
            var receiver = this.usersRepository.GetByIdAsync(username).GetAwaiter().GetResult();

            var messages = await this.messageRepository
                .All()
                .Where(u => u.Receiver == receiver)
                .To<MessageOutputModel>()
                .ToArrayAsync();

            return messages;
        }
    }
}