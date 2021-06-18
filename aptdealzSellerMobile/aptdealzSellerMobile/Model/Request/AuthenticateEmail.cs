using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class AuthenticateEmail
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("otp")]
        public string Otp { get; set; }
    }
}
