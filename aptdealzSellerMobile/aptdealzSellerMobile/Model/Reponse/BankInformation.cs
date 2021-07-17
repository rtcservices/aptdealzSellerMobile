using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class BankInformation
    {
        [JsonProperty("gstin")]
        public string Gstin { get; set; }

        [JsonProperty("pan")]
        public string Pan { get; set; }

        [JsonProperty("bankAccountNumber")]
        public string BankAccountNumber { get; set; }

        [JsonProperty("branch")]
        public string Branch { get; set; }

        [JsonProperty("ifsc")]
        public string Ifsc { get; set; }
    }
}
