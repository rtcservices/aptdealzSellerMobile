using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class ResetPassword
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
