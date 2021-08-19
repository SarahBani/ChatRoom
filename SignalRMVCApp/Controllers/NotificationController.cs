using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRMVCApp.Hubs;
using SignalRMVCApp.Models;

namespace SignalRMVCApp.Controllers
{
    public class NotificationController : Controller
    {

        #region Properties

        private readonly IHubContext<NotificationHub> _notificationHubContext;
        
        private readonly IHubContext<NotificationUserHub> _notificationUserHubContext;

        private readonly IUserConnectionManager _userConnectionManager;

        #endregion /Properties

        #region Contructors

        public NotificationController(
            IHubContext<NotificationHub> notificationHubContext,
            IHubContext<NotificationUserHub> notificationUserHubContext, 
            IUserConnectionManager userConnectionManager)
        {
            this._notificationHubContext = notificationHubContext;
            this._notificationUserHubContext = notificationUserHubContext;
            this._userConnectionManager = userConnectionManager;
        }

        #endregion /Contructors

        #region Methods

        [HttpPost]
        public void SendAll(Notification model)
        {
            this._notificationHubContext.Clients.All.SendAsync("SendAll", model);
        }

        [HttpPost]
        public void SendToSpecificUser(Notification model)
        {
            var connections = this._userConnectionManager.GetUserConnections(model.ReceiverUserId);
            if (connections != null && connections.Count > 0)
            {
                foreach (var connectionId in connections)
                {
                    this._notificationUserHubContext.Clients.Client(connectionId).SendAsync("SendToUser", model);
                }
            }
        }

        #endregion /Methods

    }
}
