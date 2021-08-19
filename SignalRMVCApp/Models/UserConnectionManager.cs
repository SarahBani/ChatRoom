using System.Collections.Generic;

namespace SignalRMVCApp.Models
{
    public class UserConnectionManager : IUserConnectionManager
    {

        #region Properties

        private static Dictionary<string, List<string>> _userConnectionMap = new Dictionary<string, List<string>>();

        private static string _userConnectionMapLocker = string.Empty;

        #endregion /Properties

        #region Methods

        public void KeepUserConnection(string userId, string connectionId)
        {
            lock (_userConnectionMapLocker)
            {
                if (!_userConnectionMap.ContainsKey(userId))
                {
                    _userConnectionMap[userId] = new List<string>();
                }
                _userConnectionMap[userId].Add(connectionId);
            }
        }

        public void RemoveUserConnection(string connectionId)
        {
            //This method will remove the connectionId of user
            lock (_userConnectionMapLocker)
            {
                foreach (var userId in _userConnectionMap.Keys)
                {
                    if (_userConnectionMap.ContainsKey(userId))
                    {
                        if (_userConnectionMap[userId].Contains(connectionId))
                        {
                            _userConnectionMap[userId].Remove(connectionId);
                            break;
                        }
                    }
                }
            }
        }

        public List<string> GetUserConnections(string userId)
        {
            var conn = new List<string>();
            lock (_userConnectionMapLocker)
            {
                conn = _userConnectionMap[userId];
            }
            return conn;
        }

        #endregion /Methods

    }
}
