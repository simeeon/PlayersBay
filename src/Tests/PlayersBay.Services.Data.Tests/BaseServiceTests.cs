namespace PlayersBay.Services.Data.Tests
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using CloudinaryDotNet;
    using PlayersBay.Data;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Data.Repositories;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data;
    using PlayersBay.Services.Mapping;
    using System.Reflection;
    using PlayersBay.Web.ViewModels.Account;

    public abstract class BaseServiceTests : IDisposable
    {
        private const string CloudinaryCloudName = "playersbay";
        private const string CloudinaryApiKey = "786522667459557";
        private const string CloudinaryApiSecret = "4dbF6YcUWfHcADOxztDfAyzxxDI";

        protected BaseServiceTests()
        {
            var services = this.SetServices();

            this.ServiceProvider = services.BuildServiceProvider();
            this.DbContext = this.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        protected IServiceProvider ServiceProvider { get; set; }

        protected ApplicationDbContext DbContext { get; set; }

        private ServiceCollection SetServices()
        {
            var services = new ServiceCollection();

            services.AddDbContext<ApplicationDbContext>(
                opt => opt.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services
                    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                        options.Password.RequiredLength = 3;
                    })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddUserStore<ApplicationUserStore>()
                    .AddRoleStore<ApplicationRoleStore>()
                    .AddDefaultTokenProviders();

            // Data repositories
            services.AddScoped(typeof(IDeletableEntityRepository<>), typeof(EfDeletableEntityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

            // Application services
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<IOffersService, OffersService>();
            services.AddScoped<IMessagesService, MessagesService>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IFeedbacksService, FeedbacksService>();

            // Identity stores
            services.AddTransient<IUserStore<ApplicationUser>, ApplicationUserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, ApplicationRoleStore>();

            // AutoMapper ????
            // AutoMapper.Mapper.Reset();
            // AutoMapperConfig.RegisterMappings(typeof(FeedbackInputModel).GetTypeInfo().Assembly);

            // Cloudinary Setup
            var cloudinaryAccount = new Account(
                CloudinaryCloudName,
                CloudinaryApiKey,
                CloudinaryApiSecret);
            var cloudinary = new Cloudinary(cloudinaryAccount);

            services.AddSingleton(cloudinary);
           
            var context = new DefaultHttpContext();
            services.AddSingleton<IHttpContextAccessor>(new HttpContextAccessor { HttpContext = context });

            return services;
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            this.SetServices();
        }
    }
}