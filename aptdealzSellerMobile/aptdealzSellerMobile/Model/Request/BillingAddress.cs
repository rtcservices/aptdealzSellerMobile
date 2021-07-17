using aptdealzSellerMobile.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace aptdealzSellerMobile.Model.Request
{
    public class BillingAddress
    {
        [JsonProperty("buildingNumber")]
        public string BuildingNumber { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("countryId")]
        public object CountryId { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        //Extra     
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public string DisplayAddress
        {
            get
            {
                List<string> billingAddress = new List<string>();

                if (!Common.EmptyFiels(BuildingNumber))
                    billingAddress.Add(BuildingNumber + Environment.NewLine);
                if (!Common.EmptyFiels(Street))
                    billingAddress.Add(Street + ", ");
                if (!Common.EmptyFiels(Landmark))
                    billingAddress.Add(Landmark + Environment.NewLine);
                if (!Common.EmptyFiels(City))
                    billingAddress.Add(City + ", ");
                if (!Common.EmptyFiels(PinCode))
                    billingAddress.Add(PinCode + Environment.NewLine);
                if (!Common.EmptyFiels(State))
                    billingAddress.Add(State + ", ");
                if (!Common.EmptyFiels(Nationality))
                    billingAddress.Add(Nationality);

                return string.Join("", billingAddress);
            }
        }

    }
}
