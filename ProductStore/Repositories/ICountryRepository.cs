using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public interface ICountryRepository
    {
        Task<IEnumerable<Country>> GetCountries();
        Task<Country> FindFirsByCountryName(string name);
        Task<Country> GetCountrytByID(long? countryId);
        void InsertCountry(Country country);
        void UpdateCountry(Country country);
        void DeleteCountry(long countryId);
        bool CountryExist(long id);
        void Save();
    }
}
