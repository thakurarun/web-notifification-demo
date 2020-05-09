using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using pushnotificationservice.Model;
using pushnotificationservice.Services;
using System.Threading.Tasks;

namespace pushnotificationservice.Controllers
{
    [EnableCors("MyPolicy")]
    public class NotificationController : Controller
    {
        private readonly IAppPushService appPushService;

        public NotificationController(IAppPushService appPushService)
        {
            this.appPushService = appPushService;
        }

        [HttpGet, Route("vapidpublickey")]
        public ActionResult<string> GetVapidPublicKey()
        {
            return Json(
                new
                {
                    PublicKey = appPushService.GetVapidPublicKey()
                });
        }

        [HttpPost("send/{userId}")]
        public async Task<ActionResult> Send([FromRoute] string userId, [FromBody] ApplicationPushMessage message)
        {
            await appPushService.Send(userId, new ClientNotification(message));
            return Ok();
        }

        [HttpPost("subscribe/{userId}")]
        public async Task<AppPushSubscription> Subscribe([FromRoute] string userId, [FromBody] PushSubscriptionViewModel model)
        {
            var subscription = new AppPushSubscription
            {
                UserId = userId,
                Endpoint = model.Subscription.Endpoint,
                ExpirationTime = model.Subscription.ExpirationTime,
                Auth = model.Subscription.Keys.Auth,
                P256Dh = model.Subscription.Keys.P256Dh
            };

            return await appPushService.Subscribe(subscription);
        }

        [HttpDelete("unsubscribe/{userId}")]
        public async Task<AppPushSubscription> Unsubscribe([FromRoute] string userId)
        {
            return await appPushService.Unsubscribe(userId);
        }
    }

    public class PushSubscriptionViewModel
    {
        public Subscription Subscription { get; set; }
        public string DeviceId { get; set; }
    }

    public class Subscription
    {
        public string Endpoint { get; set; }
        public double? ExpirationTime { get; set; }
        public Keys Keys { get; set; }
    }

    public class Keys
    {
        public string P256Dh { get; set; }
        public string Auth { get; set; }
    }
}