using Newtonsoft.Json;
using System.Collections.Generic;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class BankInformation
    {
        [JsonProperty("gstin")]
        public object Gstin { get; set; }

        [JsonProperty("pan")]
        public object Pan { get; set; }

        [JsonProperty("bankAccountNumber")]
        public object BankAccountNumber { get; set; }

        [JsonProperty("branch")]
        public object Branch { get; set; }

        [JsonProperty("ifsc")]
        public object Ifsc { get; set; }

        [JsonProperty("documents")]
        public List<string> Documents { get; set; }
    }
}
