using Microsoft.AspNetCore.SignalR;

namespace WebSite.SignalR
{
    public class RebateHub : Hub
    {
        public async Task SendMessagePage(string PageName, string message)
        {
            await Clients.All.SendAsync(PageName, message);
        }
        public async Task SendMessageToServer(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage",  message);
        }
        
    }
}
