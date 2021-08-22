using System;

namespace ServerSentEventsWebAPI.Models
{
    public class ChatMessage
    {

        #region Properties

        public string Username { get; init; }

        public string Message { get; init; }

        public DateTime SentDateTime { get; private set; }

        #endregion /Properties

        #region Constructors

        public ChatMessage(string username, string message)
        {
            this.Username = username;
            this.Message = message;
            this.SentDateTime = DateTime.Now;
            //this.SentDateTime = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
        }

        #endregion /Constructors

    }
}
