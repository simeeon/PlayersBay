namespace PlayersBay.Web.Views.ViewComponents
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using PlayersBay.Services.Data.Contracts;

    [ViewComponent(Name = "MessageList")]
    public class MessageListViewComponent : ViewComponent
    {
        private readonly IMessagesService messagesService;

        public MessageListViewComponent(IMessagesService messagesService)
        {
            this.messagesService = messagesService;
        }

        public async Task<string> InvokeAsync(string username)
        {
            var messages = await this.messagesService.GetAllMessagesAsync(username);
            var unreadMessages = messages.Where(m => m.IsRead == false).Count();
            return unreadMessages.ToString();
        }
    }
}
