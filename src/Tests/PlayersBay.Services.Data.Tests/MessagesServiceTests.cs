using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Messages;
using PlayersBay.Services.Data.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class MessagesServiceTests : BaseServiceTests
    {
        // User 1
        private readonly string FirstId = Guid.NewGuid().ToString();
        private const string Username = "admin";
        private const string Email = "admin@gmail.com";
        private const decimal InitialBalance = 100;
        private const decimal TopUpBalance = 50;
        // User 2
        private readonly string SecondId = Guid.NewGuid().ToString();
        private const string UsernameTwo = "user";
        private const string EmailTwo = "user@gmail.com";

        private const string MessageText = "Sample message text";

        private IMessagesService MessagesServiceMock => this.ServiceProvider.GetRequiredService<IMessagesService>();

        [Fact]
        public async Task CreateAsyncReturnsCreatedMessage()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            var messageInputModel = new MessageInputModel
            {
                SenderName = Username,
                ReceiverName = UsernameTwo,
                Message = MessageText,
            };

            var messageId = await this.MessagesServiceMock.CreateAsync(Username, messageInputModel);

            var message = this.DbContext.Messages.FirstOrDefault(u => u.SenderId == FirstId);

            Assert.Equal(1, messageId);
            Assert.Equal(MessageText, message.Text);
            Assert.Equal(FirstId, message.SenderId);
            Assert.Equal(SecondId, message.ReceiverId);
        }

        [Fact]
        public async Task DeleteByIdOnlyDeletesOneMessage()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            this.DbContext.Messages.Add(new Message
            {
                Id = 1,
                ReceiverId = FirstId,
                SenderId = SecondId,
                IsRead = false,
                Text = MessageText
            });

            this.DbContext.Messages.Add(new Message
            {
                Id = 2,
                ReceiverId = SecondId,
                SenderId = FirstId,
                IsRead = true,
                Text = MessageText + "2"
            });
            await this.DbContext.SaveChangesAsync();

            await this.MessagesServiceMock.DeleteAsync(FirstId, 1);

            var expectedDbSetCount = 1;
            Assert.Equal(expectedDbSetCount, this.DbContext.Messages.Count());

            var message = this.DbContext.Messages.FirstOrDefault(u => u.SenderId == FirstId);
            Assert.Equal(MessageText + "2", message.Text);
            Assert.Equal(FirstId, message.SenderId);
            Assert.Equal(SecondId, message.ReceiverId);
            Assert.Equal(2, message.Id);
        }

        [Fact]
        public async Task DeleteByIdThrowsErrorIfUserIsNotSenderOrReceiver()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);
            await this.AddTestingUserToDb("someId", "someUsername", EmailTwo);

            this.DbContext.Messages.Add(new Message
            {
                Id = 1,
                ReceiverId = FirstId,
                SenderId = SecondId,
                IsRead = false,
                Text = MessageText
            });

            this.DbContext.Messages.Add(new Message
            {
                Id = 2,
                ReceiverId = SecondId,
                SenderId = FirstId,
                IsRead = true,
                Text = MessageText + "2"
            });
            await this.DbContext.SaveChangesAsync();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                this.MessagesServiceMock.DeleteAsync("someId", 1));

            Assert.Equal(string.Format(DataConstants.InvalidDeleteMessage), exception.Message);
        }

        [Fact]
        public async Task MessageSeenChangesMessagePropToSeen()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            this.DbContext.Messages.Add(new Message
            {
                Id = 1,
                ReceiverId = FirstId,
                SenderId = SecondId,
                IsRead = false,
                Text = MessageText
            });

            await this.DbContext.SaveChangesAsync();

            var message = this.DbContext.Messages.FirstOrDefault(u => u.SenderId == SecondId);

            Assert.NotEqual(true, message.IsRead);

            await this.MessagesServiceMock.MessageSeenAsync(1);

            Assert.Equal(true, message.IsRead);
        }

        [Fact]
        public async Task GetAllAsyncReturnsAllMessagesForUser()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            this.DbContext.Messages.Add(new Message
            {
                Id = 1,
                ReceiverId = FirstId,
                SenderId = SecondId,
                IsRead = false,
                Text = MessageText
            });

            this.DbContext.Messages.Add(new Message
            {
                Id = 2,
                ReceiverId = SecondId,
                SenderId = FirstId,
                IsRead = true,
                Text = MessageText + "2"
            });
            await this.DbContext.SaveChangesAsync();

            var expected = new MessageOutputModel[]
            {
                new MessageOutputModel
                {
                    Id = 1,
                    Receiver = Username,
                    Sender = UsernameTwo,
                    IsRead = false,
                    Text = MessageText
                }
            };

            var actual = await this.MessagesServiceMock.GetAllMessagesAsync(Username);

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Receiver, elem1.Receiver);
                    Assert.Equal(expected[0].IsRead, elem1.IsRead);
                    Assert.Equal(expected[0].Text, elem1.Text);
                    Assert.Equal(expected[0].Sender, elem1.Sender);
                });
            Assert.Equal(expected.Length, actual.Length);
        }

        [Fact]
        public async Task GetOutboxAsyncReturnsAllSentMessagesForUser()
        {
            await this.AddTestingUserToDb(FirstId, Username, Email);
            await this.AddTestingUserToDb(SecondId, UsernameTwo, EmailTwo);

            this.DbContext.Messages.Add(new Message
            {
                Id = 1,
                ReceiverId = FirstId,
                SenderId = SecondId,
                IsRead = false,
                Text = MessageText
            });

            this.DbContext.Messages.Add(new Message
            {
                Id = 2,
                ReceiverId = SecondId,
                SenderId = FirstId,
                IsRead = true,
                Text = MessageText + "2"
            });
            await this.DbContext.SaveChangesAsync();

            var expected = new MessageOutputModel[]
            {
                new MessageOutputModel
                {
                    Id = 1,
                    Receiver = Username,
                    Sender = UsernameTwo,
                    IsRead = false,
                    Text = MessageText
                }
            };

            var actual = await this.MessagesServiceMock.GetOutboxAsync(UsernameTwo);

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Receiver, elem1.Receiver);
                    Assert.Equal(expected[0].IsRead, elem1.IsRead);
                    Assert.Equal(expected[0].Text, elem1.Text);
                    Assert.Equal(expected[0].Sender, elem1.Sender);
                });
            Assert.Equal(expected.Length, actual.Length);
        }

        private async Task AddTestingUserToDb(string id, string username, string email)
        {
            DbContext.Users.Add(new ApplicationUser
            {
                Id = id,
                UserName = username,
                Email = email,
                Balance = InitialBalance,
            });
            await this.DbContext.SaveChangesAsync();
        }
    }
}

