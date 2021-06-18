using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class UniquePhoneNumber
    {
        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }
    }
}
