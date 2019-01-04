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
        public IActionResult Inbox(string username)
        {
            var inbox = this.messagesService.GetAllMessagesAsync(username)
                .GetAwaiter()
                .GetResult();

            return this.View(inbox);
        }

        [HttpGet]
        public IActionResult SendMessage(string receiver)
        {
            this.ViewData["receiver"] = receiver;
            return this.View();
        }

        [HttpPost]
        public IActionResult SendMessage(MessageInputModel inputModel)
        {
            this.messagesService.CreateAsync(
                    inputModel)
                .GetAwaiter()
                .GetResult();

            return this.Redirect("/").WithSuccess("Success", "Message sent.");
        }

        [HttpGet]
        public IActionResult DeleteMessage(int id)
        {
            this.messagesService.DeleteAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("Inbox", "Messages", new { username = this.User.Identity.Name }).WithInfo("Success", $"Message with id #{id} deleted.");
        }

        [HttpGet]
        public IActionResult MessageSeen(int id)
        {
            this.messagesService.MessageSeenAsync(id)
                .GetAwaiter()
                .GetResult();

            return this.RedirectToAction("Inbox", "Messages", new { username = this.User.Identity.Name });
        }

        [HttpGet]
        public IActionResult Reply(string receiver)
        {
            this.ViewData["receiver"] = receiver;
            return this.View();
        }
    }
}
