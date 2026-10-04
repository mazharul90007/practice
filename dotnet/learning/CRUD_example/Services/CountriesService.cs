using ServiceContracts;
using ServiceContracts.DTO;
using Entities;

namespace Services;

public class CountriesService : ICountriesService
{
    //private field
    private readonly List<Country> _countries;

    //constructor
    public CountriesService()
    {
        _countries = new List<Country>();
    }


    //====================Add Country=================
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
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
        if (_countries.Any(c => c.CountryName == countryAddRequest.CountryName))
        {

            throw new ArgumentException("Country name can't be duplicate", nameof(countryAddRequest.CountryName));
        }

        //============Implementation=============

        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        //generate CountryId
        country.CountryId = Guid.NewGuid();

        //Add country object into _countries
        _countries.Add(country);

        return country.ToCountryResponse();
    }


    //===============Get All Countries=================

    public List<CountryResponse> GetAllCountries()
    {
        return _countries.Select(country => country.ToCountryResponse()).ToList();
    }

    //===============Get Country by Id==================
    public CountryResponse? GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
        {
            return null;
        }

        Country? country_response_from_list = _countries.FirstOrDefault(c => c.CountryId == countryId);

        if (country_response_from_list == null)
        {
            return null;
        }

        return country_response_from_list.ToCountryResponse();

    }
}
