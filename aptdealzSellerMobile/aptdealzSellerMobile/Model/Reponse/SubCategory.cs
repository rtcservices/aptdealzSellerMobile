using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class SubCategory
    {
        [JsonProperty("subCategoryId")]
        public string SubCategoryId { get; set; }

        [JsonProperty("categoryId")]
        public string CategoryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
