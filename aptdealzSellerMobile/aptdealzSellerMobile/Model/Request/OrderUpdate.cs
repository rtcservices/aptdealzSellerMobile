using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class OrderUpdate
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("shipperNumber")]
        public string ShipperNumber { get; set; }

        [JsonProperty("lrNumber")]
        public string LrNumber { get; set; }

        [JsonProperty("billNumber")]
        public string BillNumber { get; set; }

        [JsonProperty("trackingLink")]
        public string TrackingLink { get; set; }
    }
}
