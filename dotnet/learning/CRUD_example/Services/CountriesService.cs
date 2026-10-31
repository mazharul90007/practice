using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace Services;

public class CountriesService : ICountriesService
{
    //private field
    private readonly PersonsDbContext _db;

    //constructor
    public CountriesService(PersonsDbContext personsDbContext)
    {
        _db = personsDbContext;
    }


    //====================Add Country=================
    public async Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest)
    {
        //============Validation=============
        //countryAddRequest can't be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }
        //countryName can't be null or empty
        if (string.IsNullOrEmpty(countryAddRequest.CountryName))
        {
            throw new ArgumentException("Country name can't be null or empty", nameof(countryAddRequest.CountryName));
        }
        //countryName can't be duplicate
        if (await _db.Countries.AnyAsync(c => c.CountryName == countryAddRequest.CountryName))
        {

            throw new ArgumentException("Country name can't be duplicate", nameof(countryAddRequest.CountryName));
        }

        //============Implementation=============

        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        //generate CountryId
        country.CountryId = Guid.NewGuid();

        //Add country object into _db
        _db.Countries.Add(country);
        await _db.SaveChangesAsync();

        return country.ToCountryResponse();
    }


    //===============Get All Countries=================

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        return await _db.Countries.Select(country => country.ToCountryResponse()).ToListAsync();
    }

    //===============Get Country by Id==================
    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
        {
            return null;
        }

        Country? country_response_from_list = await _db.Countries.FirstOrDefaultAsync(c => c.CountryId == countryId);

        if (country_response_from_list == null)
        {
            return null;
        }

        return country_response_from_list.ToCountryResponse();

    }
}
