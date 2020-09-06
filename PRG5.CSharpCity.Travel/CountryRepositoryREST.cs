using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace PRG5.CSharpCity.Travel
{
    public class CountryRepositoryREST : ICountryRepository
    {
        private readonly HttpClient _httpClient;
        private const string serviceUrl = "https://restcountries.eu/rest/v2/";
        private const string allCommand = "all";
        private List<Country> countryCache;

        public CountryRepositoryREST (IHttpClientFactory httpClientFactory)
        {
            var httpClient = httpClientFactory.CreateClient("CountryRepositoryREST");
            httpClient.BaseAddress = new Uri(serviceUrl);
            _httpClient = httpClient;
        }


        public List<Country> FindAll()
        {
            List<RestCountry> queryResponse;
            if (countryCache == null)
            {
                List<Country> result = new List<Country>();
                using (HttpResponseMessage response = _httpClient.GetAsync(allCommand).GetAwaiter().GetResult())
                {
                    var responseText = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    queryResponse = JsonSerializer.Deserialize<List<RestCountry>>(responseText);
                }

                queryResponse.ForEach(c => result.Add(
                    new Country
                    {
                        Capital = c.capital,
                        EnglishName = c.name,
                        NativeName = c.nativeName,
                        ISOCode = c.alpha3Code,
                        LCID = Convert.ToInt32(c.numericCode),
                        Region = c.region,
                        SubRegion = c.subregion,
                        Currency = c.currencies.Any() ? new Currency
                        {
                            Code = c.currencies.First().code,
                            Name = c.currencies.First().name,
                            Symbol = c.currencies.First().symbol
                        } : null,
                        Languages = c.languages.Select(l => l.name).ToList()
                    }
                    ));
                countryCache = result;
            }
            return countryCache;
        }
    }
}
