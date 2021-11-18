using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Requests;

namespace CovidBot
{
    public class DataModel
    {
        public string Country { get; set; }

        public string CountryCode { get; set; }

        public string Province { get; set; }

        public string City { get; set; }

        public string CityCode { get; set; }

        public string Lat { get; set; }

        public string Lon { get; set; }

        public int Confirmed { get; set; }

        public int Deaths { get; set; }

        public int Recovered { get; set; }

        public int Active { get; set; }

        public string Status { get; set; }

        public string Cases { get; set; }

        public string Date { get; set; }

        public string LocationID { get; set; }

    }
}
