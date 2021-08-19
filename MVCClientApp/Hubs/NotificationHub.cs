using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using MVCClientApp.Models;

namespace MVCClientApp.Hubs
{
    public class NotificationHub : Hub
    {

        [HttpPost]
        public void NotifyAll(Notification notification)
        {
            Clients.All.SendAsync("SendAll", notification);
        }

    }
}
