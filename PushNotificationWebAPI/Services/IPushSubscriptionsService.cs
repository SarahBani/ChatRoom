using Lib.Net.Http.WebPush;
using System.Collections.Generic;

namespace PushNotificationWebAPI.Services
{
    public interface IPushSubscriptionsService
    {

        PushSubscription Get(string endPoint);

        IEnumerable<PushSubscription> GetAll();

        bool Contains(string endPoint);

        void Insert(PushSubscription subscription);

        void Delete(string endPoint);

    }
}
