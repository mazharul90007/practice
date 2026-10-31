using ServiceContracts.DTO;

namespace ServiceContracts;
/// <summary>
/// Represents business logic for manipulating Country entity
/// </summary>

public interface ICountriesService
{
    /// <summary>
    /// Adds a country object to the list of countries
    /// </summary>
    /// <param name="countryAddRequest">Country object to add</param>
    /// <returns>Country response object</returns>
    Task<CountryResponse> AddCountry(CountryAddRequest? countryAddRequest);

    /// <summary>
    /// Returns all the countries from the list of countries
    /// </summary>
    /// <returns>All the countries from the list as List of CountryResponse</returns>
    Task<List<CountryResponse>> GetAllCountries();

/// <summary>
/// Returns a country object based on the given country id
/// </summary>
/// <param name="CountryId"> CountryId (guid) to search</param>
/// <returns>Matching country as CountryResponse object</returns>
    Task<CountryResponse?> GetCountryByCountryId(Guid? CountryId);
}
