using Lib.Net.Http.WebPush;
using Lib.Net.Http.WebPush.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using pushnotificationservice.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace pushnotificationservice.Services
{
    public interface IAppPushService
    {
        string GetVapidPublicKey();
        Task<AppPushSubscription> Subscribe(AppPushSubscription subscription);
        Task<AppPushSubscription> Unsubscribe(string userId);
        Task Send(string userId, ClientNotification clientNotification);
    }

    public class AppPushService : IAppPushService
    {
        private readonly NotificationDbContext context;
        private readonly PushServiceClient client;
        private readonly VapidDetails vapidDetails;
        private readonly string vapidPublicKey;
        private readonly string vapidPrivateKey;

        public AppPushService(NotificationDbContext context)
        {
            this.context = context;
            this.client = new PushServiceClient();
        }

        public AppPushService(NotificationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.client = new PushServiceClient();

            var vapidSubject = configuration.GetValue<string>("Vapid:Subject");
            this.vapidPublicKey = configuration.GetValue<string>("Vapid:PublicKey");
            this.vapidPrivateKey = configuration.GetValue<string>("Vapid:PrivateKey");

            ValidateVapidKeys(vapidSubject, vapidPublicKey, vapidPrivateKey);

            vapidDetails = new VapidDetails(vapidSubject, vapidPublicKey, vapidPrivateKey);
        }

        public string GetVapidPublicKey()
        {
            return vapidDetails.VapidPublicKey;
        }

        public async Task Send(string userId, ClientNotification clientNotification)
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
            await Send(userId, new PushMessage(JsonConvert.SerializeObject(clientNotification)));
        }

        private async Task Send(string userId, PushMessage notification)
        {
            foreach (var subscription in await GetUserSubscriptions(userId))
                try
                {
                    await client.RequestPushMessageDeliveryAsync(
                        subscription.ToWebPushSubscription(),
                        notification,
                        new VapidAuthentication(vapidPublicKey, vapidPrivateKey));
                }
                catch (Exception e) when (e.Message == "Subscription no longer valid")
                {
                    context.AppPushSubscriptions.Remove(subscription);
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something wrong with message sending service.");
                    throw e;
                }
        }

        public async Task<AppPushSubscription> Subscribe(AppPushSubscription subscription)
        {
            if (await context.AppPushSubscriptions.AnyAsync(s => s.P256Dh == subscription.P256Dh))
                return await context.AppPushSubscriptions.FindAsync(subscription.P256Dh);

            await context.AppPushSubscriptions.AddAsync(subscription);
            await context.SaveChangesAsync();

            return subscription;
        }

        public async Task<AppPushSubscription> Unsubscribe(string userId)
        {
            if (!await context.AppPushSubscriptions.AnyAsync(s => s.UserId == userId))
                return null;
            var subscription = await context.AppPushSubscriptions.FirstAsync(s => s.UserId == userId);
            context.AppPushSubscriptions.Remove(subscription);
            await context.SaveChangesAsync();
            return subscription;
        }

        private async Task<List<AppPushSubscription>> GetUserSubscriptions(string userId) =>
            await context.AppPushSubscriptions.Where(s => s.UserId == userId).ToListAsync();

        private void ValidateVapidKeys(string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
        {
            if (string.IsNullOrEmpty(vapidSubject) ||
                string.IsNullOrEmpty(vapidPublicKey) ||
                string.IsNullOrEmpty(vapidPrivateKey))
            {
                throw new NotImplementedException("Vapid keys not provided.");
            }
        }
    }
}
