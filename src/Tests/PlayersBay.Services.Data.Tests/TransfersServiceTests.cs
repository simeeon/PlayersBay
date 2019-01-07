using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Data.Models.Enums;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Transfers;
using PlayersBay.Services.Data.Utilities;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class TransfersServiceTests : BaseServiceTests
    {
        private const decimal InitialBalance = 20;
        // User 1
        private readonly string FirstUserId = Guid.NewGuid().ToString();
        private const string FirstUsername = "admin";
        private const string FirstEmail = "admin@gmail.com";
        // User 2
        private readonly string SecondUserId = Guid.NewGuid().ToString();
        private const string SecondUsername = "user";
        private const string SecondEmail = "user@gmail.com";

        // Transfer 1
        private const int FirstTransferId = 1;
        private const decimal FirstTransferAmount = 10;
        private const TransferType FirstType = TransferType.Deposit;
        private const TransferStatus FirstStatus = TransferStatus.Pending;
        // Transfer 1
        private const int SecondTransferId = 2;
        private const decimal SecondTransferAmount = 20;
        private const TransferType SecondType = TransferType.Withdraw;
        private const TransferStatus SecondStatus = TransferStatus.Approved;
        // Transfer 1
        private const int ThirdTransferId = 3;
        private const decimal ThirdTransferAmount = 30;
        private const TransferType ThirdType = TransferType.Deposit;
        private const TransferStatus ThirdStatus = TransferStatus.Declined;


        private ITransfersService TransfersServiceMock => this.ServiceProvider.GetRequiredService<ITransfersService>();

        [Fact]
        public async Task CreateDepositRequestAsyncReturnsDepositWithStatusPending()
        {
            var expected = new TransferInputModel
            {
                Id = FirstTransferId,
                Amount = FirstTransferAmount,
                Type = FirstType,
                Status = FirstStatus,
            };

            var transferCreateInputModel = new TransferInputModel()
            {
                Amount = FirstTransferAmount,
                Type = FirstType,
                Status = FirstStatus,
            };

            var actual = await this.TransfersServiceMock.CreateDepositRequestAsync(FirstUsername, transferCreateInputModel);

            Assert.Equal(actual, expected.Id);
        }

        [Fact]
        public async Task CreateWithdrawalRequestAsyncReturnsWithdrawaltWithStatusPending()
        {
            var expected = new TransferInputModel
            {
                Id = FirstTransferId,
                Amount = SecondTransferAmount,
                Type = SecondType,
                Status = SecondStatus,
            };

            var transferCreateInputModel = new TransferInputModel()
            {
                Amount = SecondTransferAmount,
                Type = SecondType,
                Status = SecondStatus,
            };

            var actual = await this.TransfersServiceMock.CreateWithdrawalRequestAsync(FirstUsername, transferCreateInputModel);

            Assert.Equal(actual, expected.Id);
        }

        [Fact]
        public async Task GetAllDepositRequestsAsyncReturnsAllPendingDepositRequests()
        {
            await this.AddTestingUserToDb(FirstUserId, FirstUsername, FirstEmail);
            await this.AddTestingUserToDb(SecondUserId, SecondUsername, SecondEmail);

            await AddTestingTransferToDb(FirstTransferId, FirstTransferAmount, FirstType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(SecondTransferId, SecondTransferAmount, SecondType, SecondStatus, FirstUserId);
            await AddTestingTransferToDb(ThirdTransferId, ThirdTransferAmount, ThirdType, ThirdStatus, SecondUserId);

            var expected = new TransferViewModel[]
            {
                new TransferViewModel
                {
                    Id = FirstTransferId,
                    Amount = FirstTransferAmount,
                    Type = FirstType,
                    Status = FirstStatus,
                    UserId = FirstUserId,
                }
            };

            var actual = await this.TransfersServiceMock.GetAllDepositRequestsAsync();

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Amount, elem1.Amount);
                    Assert.Equal(expected[0].Type, elem1.Type);
                    Assert.Equal(expected[0].Status, elem1.Status);
                    Assert.Equal(expected[0].UserId, elem1.UserId);
                });
            Assert.Equal(expected.Length, actual.Length);
        }

        [Fact]
        public async Task GetAllWithdrawalRequestsAsyncReturnsAllPendingWithdrawalRequests()
        {
            await this.AddTestingUserToDb(FirstUserId, FirstUsername, FirstEmail);
            await this.AddTestingUserToDb(SecondUserId, SecondUsername, SecondEmail);

            await AddTestingTransferToDb(FirstTransferId, FirstTransferAmount, FirstType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(SecondTransferId, SecondTransferAmount, SecondType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(ThirdTransferId, ThirdTransferAmount, ThirdType, ThirdStatus, SecondUserId);

            var expected = new TransferViewModel[]
            {
                new TransferViewModel
                {
                    Id = SecondTransferId,
                    Amount = SecondTransferAmount,
                    Type = SecondType,
                    Status = FirstStatus,
                    UserId = FirstUserId,
                }
            };

            var actual = await this.TransfersServiceMock.GetAllWithdrawalRequestsAsync();

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Amount, elem1.Amount);
                    Assert.Equal(expected[0].Type, elem1.Type);
                    Assert.Equal(expected[0].Status, elem1.Status);
                    Assert.Equal(expected[0].UserId, elem1.UserId);
                });
            Assert.Equal(expected.Length, actual.Length);
        }

        [Fact]
        public async Task ApproveTransferAsyncApprovesDepositRequest()
        {
            await this.AddTestingUserToDb(FirstUserId, FirstUsername, FirstEmail);
            await this.AddTestingUserToDb(SecondUserId, SecondUsername, SecondEmail);

            await AddTestingTransferToDb(FirstTransferId, FirstTransferAmount, FirstType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(SecondTransferId, SecondTransferAmount, SecondType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(ThirdTransferId, ThirdTransferAmount, ThirdType, ThirdStatus, SecondUserId);

            var transfer = this.DbContext.Transfers.FirstOrDefault(u => u.Id == FirstTransferId);

            Assert.True(transfer.Status == TransferStatus.Pending);
            await this.TransfersServiceMock.ApproveTransferAsync(FirstTransferId);
            Assert.True(transfer.Status == TransferStatus.Approved);
        }

        [Fact]
        public async Task ApproveTransferAsyncApprovesWithdrawtRequest()
        {
            await this.AddTestingUserToDb(FirstUserId, FirstUsername, FirstEmail);
            await this.AddTestingUserToDb(SecondUserId, SecondUsername, SecondEmail);

            await AddTestingTransferToDb(FirstTransferId, FirstTransferAmount, FirstType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(SecondTransferId, SecondTransferAmount, SecondType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(ThirdTransferId, ThirdTransferAmount, ThirdType, ThirdStatus, SecondUserId);

            var transfer = this.DbContext.Transfers.FirstOrDefault(u => u.Id == SecondTransferId);

            Assert.True(transfer.Status == TransferStatus.Pending);
            await this.TransfersServiceMock.ApproveTransferAsync(SecondTransferId);
            Assert.True(transfer.Status == TransferStatus.Approved);
        }

        [Fact]
        public async Task ApproveTransferAsyncThrowsInvalidOperationExceptionIfWithdrawalAmountIsBiggerThanBalance()
        {
            await this.AddTestingUserToDb(FirstUserId, FirstUsername, FirstEmail);
            await this.AddTestingTransferToDb(FirstTransferId, ThirdTransferAmount, TransferType.Withdraw, TransferStatus.Pending, FirstUserId);

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                this.TransfersServiceMock.ApproveTransferAsync(FirstTransferId));

            Assert.Equal(string.Format(DataConstants.InvalidWithdrawalAmount, ThirdTransferAmount, FirstUsername, InitialBalance), exception.Message);
        }

        [Fact]
        public async Task GetAllTransfersAsyncReturnsAllTransfers()
        {
            await this.AddTestingUserToDb(FirstUserId, FirstUsername, FirstEmail);
            await this.AddTestingUserToDb(SecondUserId, SecondUsername, SecondEmail);

            await AddTestingTransferToDb(FirstTransferId, FirstTransferAmount, FirstType, FirstStatus, FirstUserId);
            await AddTestingTransferToDb(SecondTransferId, SecondTransferAmount, SecondType, SecondStatus, FirstUserId);
            await AddTestingTransferToDb(ThirdTransferId, ThirdTransferAmount, ThirdType, ThirdStatus, SecondUserId);

            var expected = new TransferViewModel[]
            {
                new TransferViewModel
                {
                    Id = FirstTransferId,
                    Amount = FirstTransferAmount,
                    Type = FirstType,
                    Status = FirstStatus,
                    UserId = FirstUserId,
                },
                new TransferViewModel
                {
                    Id = SecondTransferId,
                    Amount = SecondTransferAmount,
                    Type = SecondType,
                    Status = SecondStatus,
                    UserId = FirstUserId,
                }
            };

            var actual = await this.TransfersServiceMock.GetAllTransfersAsync(FirstUsername);

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Amount, elem1.Amount);
                    Assert.Equal(expected[0].Type, elem1.Type);
                    Assert.Equal(expected[0].Status, elem1.Status);
                    Assert.Equal(expected[0].UserId, elem1.UserId);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Amount, elem2.Amount);
                    Assert.Equal(expected[1].Type, elem2.Type);
                    Assert.Equal(expected[1].Status, elem2.Status);
                    Assert.Equal(expected[1].UserId, elem2.UserId);
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
                Balance = InitialBalance
            });
            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddTestingTransferToDb(
            int id,
            decimal amount,
            TransferType type,
            TransferStatus status,
            string userId)
        {
            DbContext.Transfers.Add(new Transfer
            {
                Id = id,
                Amount = amount,
                Type = type,
                Status = status,
                UserId = userId
            });
            await this.DbContext.SaveChangesAsync();
        }
    }
}
