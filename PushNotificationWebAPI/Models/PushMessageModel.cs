using Lib.Net.Http.WebPush;

namespace PushNotificationWebAPI.Models
{
    public class PushMessageModel
    {
        public string Topic { get; set; }

        public string Notification { get; set; }

        public PushMessageUrgency Urgency { get; set; } = PushMessageUrgency.Normal;

    }
}
