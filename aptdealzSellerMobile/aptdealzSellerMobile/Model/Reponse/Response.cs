using Newtonsoft.Json;

namespace aptdealzSellerMobile.Model.Reponse
{
    public class Response
    {
        [JsonProperty("succeeded")]
        public bool Succeeded { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public object Errors { get; set; }

        [JsonProperty("data")]
        public object Data { get; set; }
    }
    public class Data
    {
        public int amount { get; set; }
    }
}
