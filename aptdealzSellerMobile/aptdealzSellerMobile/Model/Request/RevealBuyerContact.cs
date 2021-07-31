using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Request
{
    public class RevealBuyerContact
    {
        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("requirementId")]
        public string RequirementId { get; set; }

        [JsonProperty("paymentStatus")]
        public int PaymentStatus { get; set; }

        [JsonProperty("razorPayPaymentId")]
        public string RazorPayPaymentId { get; set; }

        [JsonProperty("razorPayOrderId")]
        public string RazorPayOrderId { get; set; }

    }
}
