using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace PRG5.CSharpCity.Travel
{
    public class CountryRepositoryFile : ICountryRepository
    {
        private const string allCommand = "all";
        private List<Country> countryCache;

        public CountryRepositoryFile()
        {
        }


        public List<Country> FindAll()
        {
            List<RestCountry> queryResponse;
            if (countryCache == null)
            {
                List<Country> result = new List<Country>();
                var folderDetails = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\{"MasterData\\masterdata.json"}");
                var JSON = System.IO.File.ReadAllText(folderDetails);
                    queryResponse = JsonSerializer.Deserialize<List<RestCountry>>(JSON);
                

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
