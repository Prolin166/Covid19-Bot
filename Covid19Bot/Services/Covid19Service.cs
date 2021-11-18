using System.Collections.Generic;
using RestSharp;
using System.Linq;
using System;

namespace CovidBot
{
    public class Covid19Service
    {
        private RestClient _restClient;
        
        public Covid19Service(string baseUrl)
        {
            _restClient = new RestClient(baseUrl);
        }

        public CountryModel GetCases(string countryName)
        {
            try
            {
                var response = GetData(countryName);
                var result = new CountryModel(countryName);

                var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<DataModel>>(response.Content);
                var lastItem = data.Last(x => x == data[data.Count - 1]);
                var secLastItem = data.Last(x => x == data[data.Count - 2]);

                result.NewConfirmed = lastItem.Confirmed - secLastItem.Confirmed;
                result.NewActive = lastItem.Active - secLastItem.Active;
                result.NewDeaths = lastItem.Deaths - secLastItem.Deaths;

                result.TotalConfirmed = data.LastOrDefault<DataModel>().Confirmed;
                result.TotalActive = data.LastOrDefault<DataModel>().Active;
                result.TotalDeaths = data.LastOrDefault<DataModel>().Deaths;

                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler aufgetreten!");
                return new CountryModel("Error occurred!");
            }
        }

        private IRestResponse GetData(string country)
        {
            var request = new RestRequest("total/country/" + country, Method.GET);
            request.RequestFormat = DataFormat.Json;
            IRestResponse response = _restClient.Execute(request);

            return response;
        }

        public List<Country> GetCountries()
        {
            try
            {
                var request = new RestRequest("countries", Method.GET);
                request.RequestFormat = DataFormat.Json;
                IRestResponse response = _restClient.Execute(request);

                return Newtonsoft.Json.JsonConvert.DeserializeObject<List<Country>>(response.Content);
            }
            catch (Exception exp)
            {
                return new List<Country>();
            }

        }
    }
}
