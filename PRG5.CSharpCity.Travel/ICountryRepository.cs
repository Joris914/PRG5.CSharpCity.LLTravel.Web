using System.Collections.Generic;

namespace PRG5.CSharpCity.Travel
{
    public interface ICountryRepository
    {
        List<Country> FindAll();
    }
}
