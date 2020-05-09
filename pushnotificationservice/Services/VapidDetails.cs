namespace pushnotificationservice.Services
{
    internal class VapidDetails
    {
        private string VapidSubject;
        private string VapidPrivateKey;
        public string VapidPublicKey { get; private set; }

        public VapidDetails(string vapidSubject, string vapidPublicKey, string vapidPrivateKey)
        {
            this.VapidSubject = vapidSubject;
            this.VapidPublicKey = vapidPublicKey;
            this.VapidPrivateKey = vapidPrivateKey;
        }
    }
}