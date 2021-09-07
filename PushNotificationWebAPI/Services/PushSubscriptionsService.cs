using Lib.Net.Http.WebPush;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushNotificationWebAPI.Services
{
    public class PushSubscriptionsService : IPushSubscriptionsService, IDisposable
    {

        #region Properties

        private readonly LiteDatabase _db;
        private readonly ILiteCollection<PushSubscription> _collection;

        private const string _dbName = "Data/PushSubscriptionsStore.db";

        #endregion /Properties

        #region Constructors

        public PushSubscriptionsService()
        {
            this._db = new LiteDatabase(_dbName);
            this._collection = this._db.GetCollection<PushSubscription>("subscriptions");
        }

        #endregion /Constructors

        #region Methods

        public PushSubscription Get(string endPoint)
        {
            return this._collection.FindOne(q => q.Endpoint == endPoint);
        }

        public IEnumerable<PushSubscription> GetAll()
        {
            return this._collection.FindAll();
        }

        public bool Contains(string endPoint)
        {
            return this._collection.Find(q => q.Endpoint == endPoint).Any();
        }

        public void Insert(PushSubscription subscription)
        {
            this._collection.Insert(subscription);
        }

        public void Delete(string endPoint)
        {
            this._collection.DeleteMany(q => q.Endpoint == endPoint);
        }

        public void Dispose()
        {
            this._db.Dispose();
        }

        #endregion /Methods

    }
}
