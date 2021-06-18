using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class Seller
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public object PhoneNumber { get; set; }

        [JsonProperty("roles")]
        public List<string> Roles { get; set; }

        [JsonProperty("isVerified")]
        public bool IsVerified { get; set; }

        [JsonProperty("expieryDateTime")]
        public DateTime ExpieryDateTime { get; set; }

        [JsonProperty("jwToken")]
        public string JwToken { get; set; }

        [JsonProperty("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonProperty("loginTrackingKey")]
        public string LoginTrackingKey { get; set; }
    }
}
