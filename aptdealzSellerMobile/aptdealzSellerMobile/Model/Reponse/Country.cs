﻿using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class Country
    {
        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("iso")]
        public string Iso { get; set; }

        [JsonProperty("nicename")]
        public string Nicename { get; set; }

        [JsonProperty("isO3")]
        public string IsO3 { get; set; }

        [JsonProperty("numcode")]
        public int? Numcode { get; set; }

        [JsonProperty("phoneCode")]
        public int PhoneCode { get; set; }
    }
    public class State
    {
        [JsonProperty("stateId")]
        public int stateId { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
