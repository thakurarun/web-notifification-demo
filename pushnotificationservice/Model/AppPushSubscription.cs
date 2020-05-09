using Lib.Net.Http.WebPush;
using System.ComponentModel.DataAnnotations;

namespace pushnotificationservice.Model
{
    public class AppPushSubscription
    {
        public AppPushSubscription() { }

        public AppPushSubscription(string userId, PushSubscription subscription)
        {
            UserId = userId;
            Endpoint = subscription.Endpoint;
            ExpirationTime = null;
            P256Dh = subscription.GetKey(PushEncryptionKeyName.P256DH);
            Auth = subscription.GetKey(PushEncryptionKeyName.Auth);
        }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string Endpoint { get; set; }

        public double? ExpirationTime { get; set; }

        [Required]
        [Key]
        public string P256Dh { get; set; }


        [Required]
        public string Auth { get; set; }

        public PushSubscription ToWebPushSubscription()
        {
            var pushSubscription = new PushSubscription();
            pushSubscription.SetKey(PushEncryptionKeyName.Auth, Auth);
            pushSubscription.SetKey(PushEncryptionKeyName.P256DH, P256Dh);
            pushSubscription.Endpoint = Endpoint;
            return pushSubscription;
        }
    }
}
