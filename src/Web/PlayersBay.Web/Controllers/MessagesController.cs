namespace PlayersBay.Web.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Common.Extensions.Alerts;
    using PlayersBay.Data.Common.Repositories;
    using PlayersBay.Data.Models;
    using PlayersBay.Services.Data.Contracts;
    using PlayersBay.Services.Data.Models.Messages;
    using PlayersBay.Services.Data.Utilities;

    [Authorize]
    public class MessagesController : BaseController
    {
        private readonly IMessagesService messagesService;
        private readonly IRepository<Message> messageRepository;
        private readonly UserManager<ApplicationUser> userManager;

        public MessagesController(IMessagesService messageService, IRepository<Message> messageRepository, UserManager<ApplicationUser> userManager)
        {
            this.messagesService = messageService;
            this.messageRepository = messageRepository;
            this.userManager = userManager;
        }

        [HttpGet]
        public IActionResult Inbox()
        {
            var username = this.User.Identity.Name;

            var inbox = this.messagesService.GetAllMessagesAsync(username).GetAwaiter().GetResult();

            return this.View(inbox);
        }

        [HttpGet]
        public IActionResult Outbox()
        {
            var username = this.User.Identity.Name;

            var outbox = this.messagesService.GetOutboxAsync(username).GetAwaiter().GetResult();

            return this.View(outbox);
        }

        [HttpGet]
        public IActionResult SendMessage(string receiver)
        {
            this.ViewData["receiver"] = receiver;

            return this.View("Reply");
        }

        [HttpPost]
        public IActionResult SendMessage(MessageInputModel inputModel)
        {
            var senderUsername = this.User.Identity.Name;

            this.messagesService.CreateAsync(senderUsername, inputModel).GetAwaiter().GetResult();

            return this.RedirectToAction("Outbox", "Messages").WithSuccess(DataConstants.NotificationMessages.Success, string.Format(DataConstants.NotificationMessages.MessageSent, inputModel.ReceiverName));
        }

        [HttpGet]
        public IActionResult DeleteMessage(int id)
        {
            var user = this.userManager.GetUserAsync(this.HttpContext.User);

            this.messagesService.DeleteAsync(user.Result.Id, id).GetAwaiter().GetResult();

            return this.RedirectToAction("Inbox", "Messages").WithInfo(DataConstants.NotificationMessages.Info, string.Format(DataConstants.NotificationMessages.MessageDeleted, id));
        }

        [HttpGet]
        public IActionResult MessageSeen(int id)
        {
            this.messagesService.MessageSeenAsync(id).GetAwaiter().GetResult();

            return this.RedirectToAction("Inbox", "Messages");
        }

        [HttpGet]
        public IActionResult Reply(string receiver)
        {
            this.ViewData["receiver"] = receiver;

            return this.View();
        }
    }
}
