using System;
using System.Collections.Generic;
using System.Text;

namespace CovidBot
{
    public class CountryModel
    {

        public CountryModel(string countryName)
        {
            Name = countryName;
        }

        public string Name { get; set; }

        public int NewActive { get; set; }

        public int NewConfirmed { get; set; }

        public int NewDeaths { get; set; }

        public int TotalActive { get; set; }

        public int TotalConfirmed { get; set; }

        public int TotalDeaths { get; set; }

    }
}
