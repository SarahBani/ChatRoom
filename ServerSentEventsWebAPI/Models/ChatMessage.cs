using System;

namespace ServerSentEventsWebAPI.Models
{
    public record ChatMessage
    {

        #region Properties

        public string Username { get; init; }

        public string Message { get; init; }

        public DateTime DateTime { get; init; }

        public bool IsNotification { get; init; }

        #endregion /Properties

        #region Constructors

        public ChatMessage(string username, string message, bool isNotification = false)
        {
            this.Username = username;
            this.Message = message;
            this.DateTime = DateTime.Now;
            //this.DateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
            this.IsNotification = isNotification;
        }

        #endregion /Constructors

    }
}
