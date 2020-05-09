using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace pushnotificationservice.Model
{
    public class ClientNotification
    {
        [JsonProperty("notification")]
        public readonly ApplicationPushMessage Notification;

        public ClientNotification(ApplicationPushMessage message)
        {
            Notification = message;
        }
    }

    public class ApplicationPushMessage
    {
        [JsonProperty("title")]
        public string Title { get; set; } = "Notification From My Application";
        [JsonProperty("lang")]
        public string Lang { get; set; } = "en";

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("badge")]
        public string Badge { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; } = DateTime.Now;

        [JsonProperty("requireInteraction")]
        public bool RequireInteraction { get; set; } = false;

        [JsonProperty("actions")]
        public List<NotificationAction> Actions { get; set; } = new List<NotificationAction>();
    }

    public class NotificationAction
    {

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }
    }
}
