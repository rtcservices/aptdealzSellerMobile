using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class RaiseGrievance
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("grievanceType")]
        public int GrievanceType { get; set; }

        [JsonProperty("issueDescription")]
        public string IssueDescription { get; set; }

        [JsonProperty("preferredSolution")]
        public string PreferredSolution { get; set; }

        [JsonProperty("documents")]
        public List<string> Documents { get; set; }
    }
}
