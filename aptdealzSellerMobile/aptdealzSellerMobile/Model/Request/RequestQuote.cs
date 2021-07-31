using Newtonsoft.Json;
using System;

namespace aptdealzSellerMobile.Model.Request
{
    public class RequestQuote
    {
        [JsonProperty("requirementId")]
        public string RequirementId { get; set; }

        [JsonProperty("quoteId")]
        public string QuoteId { get; set; }

        [JsonProperty("unitPrice")]
        public decimal UnitPrice { get; set; }

        [JsonProperty("handlingCharges")]
        public decimal HandlingCharges { get; set; }

        [JsonProperty("shippingCharges")]
        public decimal ShippingCharges { get; set; }

        [JsonProperty("insuranceCharges")]
        public decimal InsuranceCharges { get; set; }

        [JsonProperty("shippingPinCode")]
        public string ShippingPinCode { get; set; }

        [JsonProperty("validityDate")]
        public DateTime ValidityDate { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }
    }
}
