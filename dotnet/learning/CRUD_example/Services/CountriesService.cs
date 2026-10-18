using ServiceContracts;
using ServiceContracts.DTO;
using Entities;

namespace Services;

public class CountriesService : ICountriesService
{
    //private field
    private readonly List<Country> _countries;

    //constructor
    public CountriesService(bool initialize = true)
    {
        _countries = new List<Country>();
        if (initialize)
        {
            _countries.AddRange(new List<Country>
            {
                new Country { CountryId = Guid.Parse("d65be9ce-46d1-4f8b-b132-bd0a024ba436"), CountryName = "USA" },
                new Country { CountryId = Guid.Parse("647b7454-94a5-4e7f-a83e-02d6ec920f82"), CountryName = "Canada" },
                new Country { CountryId = Guid.Parse("d67e5387-0d35-4784-b7c5-6f77b1a59536"), CountryName = "UK" },
                new Country { CountryId = Guid.Parse("f06919f2-43c6-4255-8523-4d1bc30a01b6"), CountryName = "India" },
                new Country { CountryId = Guid.Parse("2ed188ec-0464-4a99-b723-996b2283d4c6"), CountryName = "Australia" }
            });
        }
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
