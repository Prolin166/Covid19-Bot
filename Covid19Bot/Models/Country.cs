using Newtonsoft.Json;

namespace CovidBot
{
    public class Country
    {
        [JsonProperty("Country")]
        public string CountryName { get; set; }
        public string Slug { get; set; }
        public string ISO2 { get; set; }

    }
}
