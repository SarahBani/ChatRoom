using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SignalRWebAPI.Hubs
{
    public class ChatHub : Hub
    {

        public async Task SendMessage(string user, string message)
        {
            var chatItem = new
            {
                user = user,
                message = message,
                dateTime = DateTime.Now
            };
            await Clients.All.SendAsync("ReceiveMessage", chatItem);
        }

    }
}
