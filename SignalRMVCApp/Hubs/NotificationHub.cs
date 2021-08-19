using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRMVCApp.Models;

namespace SignalRMVCApp.Hubs
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
