using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class RatingReview
    {
        [JsonProperty("reviewForOrderId")]
        public string ReviewForOrderId { get; set; }

        [JsonProperty("reviewedForSellerId")]
        public string ReviewedForSellerId { get; set; }

        [JsonProperty("sellerRating")]
        public int SellerRating { get; set; }
    }
}
