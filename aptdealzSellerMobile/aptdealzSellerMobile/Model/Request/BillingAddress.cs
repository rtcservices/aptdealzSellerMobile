using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class BillingAddress
    {
        [JsonProperty("buildingNumber")]
        public string BuildingNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        //Extra     
        [JsonIgnore]
        public int Id { get; set; }
    }
}
