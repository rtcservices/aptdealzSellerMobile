using aptdealzSellerMobile.Model.Reponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace aptdealzSellerMobile.Model.Request
{
    public class SellerDetails
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("sellerId")]
        public string SellerId { get; set; }

        [JsonProperty("sellerNo")]
        public string SellerNo { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("profilePhoto")]
        public string ProfilePhoto { get; set; }

        //[JsonProperty("alternativePhoneNumber")]
        //public object AlternatePhoneNumber { get; set; }

        [JsonProperty("building")]
        public string Building { get; set; }

        [JsonProperty("street")]
        public string Street { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("landmark")]
        public string Landmark { get; set; }

        [JsonProperty("pinCode")]
        public string PinCode { get; set; }

        [JsonProperty("countryId")]
        public int CountryId { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("companyProfile")]
        public CompanyProfile CompanyProfile { get; set; }

        [JsonProperty("bankInformation")]
        public BankInformation BankInformation { get; set; }

        [JsonProperty("billingAddresses")]
        public List<BillingAddress> BillingAddresses { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("alternativePhoneNumber")]
        public string AlternativePhoneNumber { get; set; }

        [JsonProperty("about")]
        public string About { get; set; }

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

        [JsonProperty("documents")]
        public List<string> Documents { get; set; }

        [JsonProperty("commissionRate")]
        public int CommissionRate { get; set; }
    }
}
