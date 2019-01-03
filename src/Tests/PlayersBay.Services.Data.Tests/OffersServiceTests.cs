using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using PlayersBay.Data.Models;
using PlayersBay.Services.Data.Contracts;
using PlayersBay.Services.Data.Models.Offers;
using PlayersBay.Services.Data.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace PlayersBay.Services.Data.Tests
{
    public class OffersServiceTests : BaseServiceTests
    {
        private const string Username = "admin";

        private const string GameName = "Diablo";
        // First offer info
        private const int FirstId = 1;
        private const string OfferTitle = "Title for my offer";
        private const string OfferDescription = "Description for my offer";
        private const decimal OfferPrice = 15.90m;
        private const string OfferMessagetoBuyer = "Message to buyer";
        // Second offer info
        private const int SecondId = 2;
        private const string SecondOfferTitle = "Second Title for my offer";
        private const string SecondOfferDescription = "Second Description for my offer";
        private const decimal SecondOfferPrice = 35.90m;
        private const string SecondOfferMessagetoBuyer = "Second Message to buyer";
        // Third offer info
        private const int ThirdId = 3;
        private const string ThirdOfferTitle = "Third Title for my offer";
        private const string ThirdOfferDescription = "Third Description for my offer";
        private const decimal ThirdOfferPrice = 45.90m;
        private const string ThirdOfferMessagetoBuyer = "Third Message to buyer";
        // Forth offer info
        private const int ForthId = 4;
        private const string ForthOfferTitle = "Forth Title for my offer";
        private const string ForthOfferDescription = "Forth Description for my offer";
        private const decimal ForthOfferPrice = 55.90m;
        private const string ForthOfferMessagetoBuyer = "Forth Message to buyer";

        private readonly string testUserId = Guid.NewGuid().ToString();

        private const string DefaultImage = "http://icons.iconarchive.com/icons/guillendesign/variations-3/256/Default-Icon-icon.png";
        private const string NewImage = "newImage.png";

        private IOffersService OffersServiceMock => this.ServiceProvider.GetRequiredService<IOffersService>();

        [Fact]
        public async Task CreateAsyncReturnsCreatedOffer()
        {
            var expected = new OfferViewModel
            {
                Id = FirstId,
                Description = OfferDescription,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
            };

            using (var stream = File.OpenRead(NewImage))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "png",
                };

                var offerCreateInputModel = new OfferCreateInputModel()
                {
                    Author = Username,
                    Description = OfferDescription,
                    GameId = FirstId,
                    Price = OfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Title = OfferTitle,
                    MessageToBuyer = OfferMessagetoBuyer,
                    ImageUrl = file,
                };

                var actual = await this.OffersServiceMock.CreateAsync(offerCreateInputModel);

                Assert.Equal(actual, expected.Id);
            }
        }

        [Fact]
        public async Task DeleteByIdOnlyDeletesOneOffer()
        {
            await this.AddTestingOfferToDb(testUserId);

            var secondOfferToDelete = new Offer
            {
                Description = "Offer to delete",
                GameId = 1,
                Id = 2,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            };

            this.DbContext.Offers.Add(secondOfferToDelete);
            await this.DbContext.SaveChangesAsync();

            await this.OffersServiceMock.DeleteAsync(secondOfferToDelete.Id);

            var expectedDbSetCount = 1;
            Assert.Equal(expectedDbSetCount, this.DbContext.Offers.Count());
        }

        [Fact]
        public async Task EditAsyncEditsOfferWhenImageStaysTheSame()
        {
            await this.AddTestingOfferToDb(testUserId);

            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                GameId = FirstId,
                Id = 2,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            });
            await this.DbContext.SaveChangesAsync();

            var newDescription = "Edited description";
            var newOfferId = 2;

            Assert.NotEqual(newDescription, this.DbContext.Offers.Find(1).Description);
            Assert.NotEqual(newOfferId, this.DbContext.Offers.Find(1).Id);

            var offerEditViewModel = new OfferToEditViewModel()
            {
                Description = newDescription,
                GameId = FirstId,
                Id = FirstId,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            };

            await this.OffersServiceMock.EditAsync(offerEditViewModel);

            Assert.Equal(newDescription, this.DbContext.Offers.Find(1).Description);
        }

        [Fact]
        public async Task EditAsyncEditsOfferImage()
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                GameId = FirstId,
                Id = FirstId,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            });
            await this.DbContext.SaveChangesAsync();

            using (var stream = File.OpenRead(NewImage))
            {
                var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "png",
                };

                var offerToEditViewModel = new OfferToEditViewModel()
                {
                    Description = OfferDescription,
                    GameId = FirstId,
                    Id = FirstId,
                    Price = OfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Status = PlayersBay.Data.Models.Enums.Status.Active,
                    Title = OfferTitle,
                    MessageToBuyer = OfferMessagetoBuyer,
                    NewImage = file,
                };

                await this.OffersServiceMock.EditAsync(offerToEditViewModel);

                ApplicationCloudinary.DeleteImage(ServiceProvider.GetRequiredService<Cloudinary>(), offerToEditViewModel.Title);
            }

            Assert.NotEqual(NewImage, this.DbContext.Offers.Find(FirstId).ImageUrl);
        }

        [Fact]
        public async Task GetAllAsyncReturnsAllOffers()
        {
            await this.AddTestingOfferToDb(testUserId);
            await this.AddSecondTestingOfferToDb(testUserId);
            await this.AddThirdTestingOfferToDb(testUserId);
            await this.AddForthTestingOfferToDb(testUserId);

            var expected = new OfferViewModel[]
            {
                new OfferViewModel
                {
                    AuthorId = testUserId,
                    Description = OfferDescription,
                    Id = FirstId,
                    Price = OfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Status = PlayersBay.Data.Models.Enums.Status.Active,
                    Title = OfferTitle,
                    MessageToBuyer = OfferMessagetoBuyer,
                    ImageUrl = DefaultImage,
                },
                new OfferViewModel
                {
                    AuthorId = testUserId,
                    Description = SecondOfferDescription,
                    Id = SecondId,
                    Price = SecondOfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Status = PlayersBay.Data.Models.Enums.Status.Active,
                    Title = SecondOfferTitle,
                    MessageToBuyer = SecondOfferMessagetoBuyer,
                    ImageUrl = DefaultImage,
                },
            };

            var actual = await this.OffersServiceMock.GetAllOffersAsync(1);

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].AuthorId, elem1.AuthorId);
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].Description, elem1.Description);
                    Assert.Equal(expected[0].MessageToBuyer, elem1.MessageToBuyer);
                    Assert.Equal(expected[0].Title, elem1.Title);
                    Assert.Equal(expected[0].Status, elem1.Status);
                    Assert.Equal(expected[0].OfferType, elem1.OfferType);
                    Assert.Equal(expected[0].Price, elem1.Price);
                    Assert.Equal(expected[0].ImageUrl, elem1.ImageUrl);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].AuthorId, elem2.AuthorId);
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Description, elem2.Description);
                    Assert.Equal(expected[1].MessageToBuyer, elem2.MessageToBuyer);
                    Assert.Equal(expected[1].Title, elem2.Title);
                    Assert.Equal(expected[1].Status, elem2.Status);
                    Assert.Equal(expected[1].OfferType, elem2.OfferType);
                    Assert.Equal(expected[1].Price, elem2.Price);
                    Assert.Equal(expected[1].ImageUrl, elem2.ImageUrl);
                });

            Assert.Equal(expected.Length, actual.Length);
        }

        [Fact]
        public async Task GetMyActiveOfferAsyncReturnsAllMyActiveOffers()
        {
            this.DbContext.Users.Add(new ApplicationUser { Id = testUserId, UserName = Username });
            await this.DbContext.SaveChangesAsync();

            await this.AddTestingOfferToDb(testUserId);
            await this.AddSecondTestingOfferToDb(testUserId);
            await this.AddThirdTestingOfferToDb(testUserId);
            await this.AddForthTestingOfferToDb(testUserId);

            var expected = new OfferViewModel[]
            {
                new OfferViewModel
                {
                    Description = OfferDescription,
                    Id = FirstId,
                    AuthorId = testUserId,
                    Price = OfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Status = PlayersBay.Data.Models.Enums.Status.Active,
                    Title = OfferTitle,
                    MessageToBuyer = OfferMessagetoBuyer,
                    ImageUrl = DefaultImage,
                },
                new OfferViewModel
                {
                    Description = SecondOfferDescription,
                    Id = SecondId,
                    AuthorId = testUserId,
                    Price = SecondOfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Status = PlayersBay.Data.Models.Enums.Status.Active,
                    Title = SecondOfferTitle,
                    MessageToBuyer = SecondOfferMessagetoBuyer,
                    ImageUrl = DefaultImage,
                },
            };
            
            var actual = await this.OffersServiceMock.GetMyActiveOffersAsync(Username);

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].AuthorId, elem1.AuthorId);
                    Assert.Equal(expected[0].Description, elem1.Description);
                    Assert.Equal(expected[0].MessageToBuyer, elem1.MessageToBuyer);
                    Assert.Equal(expected[0].Title, elem1.Title);
                    Assert.Equal(expected[0].Status, elem1.Status);
                    Assert.Equal(expected[0].OfferType, elem1.OfferType);
                    Assert.Equal(expected[0].Price, elem1.Price);
                    Assert.Equal(expected[0].ImageUrl, elem1.ImageUrl);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].AuthorId, elem2.AuthorId);
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].Description, elem2.Description);
                    Assert.Equal(expected[1].MessageToBuyer, elem2.MessageToBuyer);
                    Assert.Equal(expected[1].Title, elem2.Title);
                    Assert.Equal(expected[1].Status, elem2.Status);
                    Assert.Equal(expected[1].OfferType, elem2.OfferType);
                    Assert.Equal(expected[1].Price, elem2.Price);
                    Assert.Equal(expected[1].ImageUrl, elem2.ImageUrl);
                });

            Assert.Equal(expected.Length, actual.Length);
        }

        [Fact]
        public async Task GetMySoldOfferAsyncReturnsAllMySoldOffers()
        {
            this.DbContext.Users.Add(new ApplicationUser { Id = testUserId, UserName = Username });
            await this.DbContext.SaveChangesAsync();

            await this.AddTestingOfferToDb(testUserId);
            await this.AddSecondTestingOfferToDb(testUserId);
            await this.AddThirdTestingOfferToDb(testUserId);
            await this.AddForthTestingOfferToDb(testUserId);

            var expected = new OfferViewModel[]
            {
                new OfferViewModel
                {
                    Description = ThirdOfferDescription,
                    Id = ThirdId,
                    AuthorId = testUserId,
                    Price = ThirdOfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Status = PlayersBay.Data.Models.Enums.Status.Completed,
                    Title = ThirdOfferTitle,
                    MessageToBuyer = ThirdOfferMessagetoBuyer,
                    ImageUrl = DefaultImage,
                },
                new OfferViewModel
                {
                    Description = ForthOfferDescription,
                    Id = ForthId,
                    AuthorId = testUserId,
                    Price = ForthOfferPrice,
                    OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                    Status = PlayersBay.Data.Models.Enums.Status.Completed,
                    Title = ForthOfferTitle,
                    MessageToBuyer = ForthOfferMessagetoBuyer,
                    ImageUrl = DefaultImage,
                },
            };

            var actual = await this.OffersServiceMock.GetMySoldOffersAsync(Username);

            Assert.Collection(actual,
                elem1 =>
                {
                    Assert.Equal(expected[0].Id, elem1.Id);
                    Assert.Equal(expected[0].AuthorId, elem1.AuthorId);
                    Assert.Equal(expected[0].Description, elem1.Description);
                    Assert.Equal(expected[0].MessageToBuyer, elem1.MessageToBuyer);
                    Assert.Equal(expected[0].Title, elem1.Title);
                    Assert.Equal(expected[0].Status, elem1.Status);
                    Assert.Equal(expected[0].OfferType, elem1.OfferType);
                    Assert.Equal(expected[0].Price, elem1.Price);
                    Assert.Equal(expected[0].ImageUrl, elem1.ImageUrl);
                },
                elem2 =>
                {
                    Assert.Equal(expected[1].Id, elem2.Id);
                    Assert.Equal(expected[1].AuthorId, elem2.AuthorId);
                    Assert.Equal(expected[1].Description, elem2.Description);
                    Assert.Equal(expected[1].MessageToBuyer, elem2.MessageToBuyer);
                    Assert.Equal(expected[1].Title, elem2.Title);
                    Assert.Equal(expected[1].Status, elem2.Status);
                    Assert.Equal(expected[1].OfferType, elem2.OfferType);
                    Assert.Equal(expected[1].Price, elem2.Price);
                    Assert.Equal(expected[1].ImageUrl, elem2.ImageUrl);
                });

            Assert.Equal(expected.Length, actual.Length);
        }

        [Fact]
        public async Task GetViewModelByIdAsyncReturnsCorrectViewModel()
        {
            await this.AddTestingGameToDb();
            await this.AddTestingOfferToDb(testUserId);

            var expected = this.DbContext.Offers.OrderBy(r => r.CreatedOn);

            var actual = await this.OffersServiceMock.GetViewModelAsync<OfferToEditViewModel>(FirstId);

            Assert.IsType<OfferToEditViewModel>(actual);
            Assert.Collection(expected,
                elem1 =>
                {
                    Assert.Equal(expected.First().Id, actual.Id);
                    Assert.Equal(expected.First().AuthorId, actual.AuthorId);
                    Assert.Equal(expected.First().MessageToBuyer, actual.MessageToBuyer);
                    Assert.Equal(expected.First().OfferType, actual.OfferType);
                    Assert.Equal(expected.First().Price, actual.Price);
                    Assert.Equal(expected.First().Title, actual.Title);
                    Assert.Equal(expected.First().Description, actual.Description);
                    Assert.Equal(expected.First().Status, actual.Status);
                    Assert.Equal(expected.First().GameId, actual.GameId);
                    Assert.Equal(expected.First().ImageUrl, actual.ImageUrl);
                });
        }

        private async Task AddTestingGameToDb()
        {
            this.DbContext.Games.Add(new Game
            {
                Id = FirstId,
                Name = GameName,
            });
            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddTestingOfferToDb(string userId)
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = OfferDescription,
                GameId = FirstId,
                Id = FirstId,
                AuthorId = userId,
                Price = OfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = OfferTitle,
                MessageToBuyer = OfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            });
            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddSecondTestingOfferToDb(string userId)
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = SecondOfferDescription,
                GameId = FirstId,
                AuthorId = userId,
                Id = SecondId,
                Price = SecondOfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Active,
                Title = SecondOfferTitle,
                MessageToBuyer = SecondOfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            });
            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddThirdTestingOfferToDb(string userId)
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = ThirdOfferDescription,
                GameId = FirstId,
                AuthorId = userId,
                Id = ThirdId,
                Price = ThirdOfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Completed,
                Title = ThirdOfferTitle,
                MessageToBuyer = ThirdOfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            });
            await this.DbContext.SaveChangesAsync();
        }

        private async Task AddForthTestingOfferToDb(string userId)
        {
            this.DbContext.Offers.Add(new Offer
            {
                Description = ForthOfferDescription,
                GameId = FirstId,
                AuthorId = userId,
                Id = ForthId,
                Price = ForthOfferPrice,
                OfferType = PlayersBay.Data.Models.Enums.OfferType.Items,
                Status = PlayersBay.Data.Models.Enums.Status.Completed,
                Title = ForthOfferTitle,
                MessageToBuyer = ForthOfferMessagetoBuyer,
                ImageUrl = DefaultImage,
            });
            await this.DbContext.SaveChangesAsync();
        }
    }
}
