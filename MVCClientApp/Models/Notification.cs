namespace MVCClientApp.Models
{
    public class Notification
    {

        #region Properties

        public string UserId { get; init; }

        public string ReceiverUserId { get; init; }

        public string Header { get; init; }

        public string Content { get; init; }

        #endregion /Properties

        #region Constructors

        public Notification()
        {

        }

        public Notification(string userId, string header, string content)
        {
            this.UserId = userId;
            this.Header = header;
            this.Content = content;
        }

        public Notification(string userId, string receiverUserId, string header, string content)
        {
            this.UserId = userId;
            this.ReceiverUserId = receiverUserId;
            this.Header = header;
            this.Content = content;
        }

        #endregion /Constructors

    }
}
