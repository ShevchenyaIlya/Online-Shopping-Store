using Microsoft.EntityFrameworkCore;
using ProductStore.Data;
using ProductStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductStore.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AuthDbContext _dbContext;

        public CountryRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Country> FindFirsByCountryName(string name)
        {
            return await _dbContext.Country.Where(value => value.CountryName == name).FirstAsync();
        }

        public async Task<IEnumerable<Country>> GetCountries()
        {
            return await _dbContext.Country.ToListAsync();
        }

        public async Task<Country> GetCountrytByID(long? countryId)
        {
            return await _dbContext.Country.FirstOrDefaultAsync(m => m.CountryId == countryId);
        }
        public bool CountryExist(long id)
        {
            return _dbContext.Country.Any(e => e.CountryId == id);
        }

        public void DeleteCountry(long countryId)
        {
            var country = _dbContext.Country.Find(countryId);
            _dbContext.Country.Remove(country);
            Save();
        }

        public void UpdateCountry(Country country)
        {
            _dbContext.Entry(country).State = EntityState.Modified;
            Save();
        }

        public void InsertCountry(Country country)
        {
            _dbContext.Add(country);
            Save();
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

    }
}
