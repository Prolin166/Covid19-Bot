using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
