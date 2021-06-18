using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class UniqueEmail
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}
