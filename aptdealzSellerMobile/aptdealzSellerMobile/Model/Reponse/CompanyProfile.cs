using Newtonsoft.Json;
using System.Collections.Generic;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class CompanyProfile
    {
        [JsonProperty("description")]
        public object Description { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("subCategories")]
        public List<string> SubCategories { get; set; }

        [JsonProperty("experience")]
        public object Experience { get; set; }

        [JsonProperty("areaOfSupply")]
        public object AreaOfSupply { get; set; }
    }
}
