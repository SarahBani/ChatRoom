using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PushNotificationWebAPI.Models;
using PushNotificationWebAPI.Services;
using PushNotificationWebAPI.Settings;
using System.Linq;

namespace PushNotificationWebAPI.Controllers
{
    [Route("push-notifications-api")]
    [ApiController]
    public class PushNotificationsController : ControllerBase
    {

        #region Properties

        //private readonly IConfiguration _configuration;

        private readonly VAPID _options;

        private readonly IPushSubscriptionsService _pushSubscriptionsService;

        private readonly PushServiceClient _pushServiceClient;

        #endregion /Properties

        #region Constructors

        public PushNotificationsController(//IConfiguration configuration,
            IOptions<VAPID> options,
            IPushSubscriptionsService pushSubscriptionsService,
            PushServiceClient pushServiceClient)
        {
            //this.configuration = configuration;
            this._options = options.Value;
            this._pushSubscriptionsService = pushSubscriptionsService;
            this._pushServiceClient = pushServiceClient;
            this._pushServiceClient.DefaultAuthentication = new VapidAuthentication(options.Value.PublicKey, options.Value.PrivateKey)
            {
                Subject = options.Value.Subject
            };
        }

        #endregion /Constructors

        #region Methods

        [HttpGet("public-key")]
        public ContentResult Get()
        {
            return Content(this._options.PublicKey, "text/plain");
        }

        [HttpPost("[action]")]
        public IActionResult Subscribe([FromBody] PushSubscription model)
        {
            if (!this._pushSubscriptionsService.Contains(model.Endpoint))
            {
                this._pushSubscriptionsService.Insert(model);
            }
            return Ok();
        }

        [HttpDelete("{endpoint}")]
        public void Unsubscribe(string endpoint)
        {
            this._pushSubscriptionsService.Delete(endpoint);
        }

        [HttpPost("notifications")]
        public IActionResult SendNotification([FromBody] PushMessageModel model)
        {
            //try
            //{
            var pushMessage = new PushMessage(model.Notification)
            {
                Topic = model.Topic,
                Urgency = model.Urgency
            };
            var webPushClient = new WebPush.WebPushClient();
            var vapidDetails = new WebPush.VapidDetails(this._options.Subject, this._options.PublicKey, this._options.PrivateKey);
            webPushClient.SetVapidDetails(vapidDetails);
            var subscriptions = this._pushSubscriptionsService.GetAll().ToList();

            foreach (PushSubscription subscription in subscriptions)
            {
                //var pushSubscription = new WebPush.PushSubscription(subscription.Endpoint, subscription.Keys["p256dh"], subscription.Keys["auth"]);
                //webPushClient.SendNotificationAsync(pushSubscription, JsonConvert.SerializeObject(model), vapidDetails);
                // webPushClient.SendNotificationAsync(pushSubscription, JsonConvert.SerializeObject(model));


                var vapidAuthentication = new VapidAuthentication(this._options.PublicKey, this._options.PrivateKey)
                {
                    Subject = this._options.Subject
                };
                this._pushServiceClient.RequestPushMessageDeliveryAsync(
                     subscription,
                     pushMessage,
                     vapidAuthentication);
            }
            //}
            //catch (WebPush.WebPushException exception)
            //{
            //    var statusCode = exception.StatusCode;
            //    //  return new StatusCodeResult((int)statusCode);
            //}
            //catch (Exception ex)
            //{
            //}

            return NoContent();
        }

        #endregion /Methods

    }
}
