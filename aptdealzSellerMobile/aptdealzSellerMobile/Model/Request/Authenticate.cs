using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class Authenticate
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
