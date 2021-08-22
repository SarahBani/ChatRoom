using System.Collections.Generic;

namespace SignalRMVCApp.Helper
{
    public interface IUserConnectionManager
    {
        void KeepUserConnection(string userId, string connectionId);

        void RemoveUserConnection(string connectionId);

        List<string> GetUserConnections(string userId);

    }
}
