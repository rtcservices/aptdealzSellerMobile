using Newtonsoft.Json;
using System.Collections.Generic;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class CompanyProfile
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("subCategories")]
        public List<string> SubCategories { get; set; }

        [JsonProperty("experience")]
        public string Experience { get; set; }

        [JsonProperty("areaOfSupply")]
        public string AreaOfSupply { get; set; }

        [JsonProperty("commissionRate")]
        public decimal CommissionRate { get; set; }
    }
}
