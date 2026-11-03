using Entities;

namespace RepositoryContracts;

/// <summary>
/// Represent data access logic for managing Person entity
/// </summary>
public interface ICountriesRepository
{
    /// <summary>
    /// Adds a new country object to the data store
    /// </summary>
    /// <param name="country">Country Object to add</param>
    /// <returns>Returns the country object after adding it to the data store</returns>
    Task<Country> AddCountry(Country country);

    /// <summary>
    /// Returns all countries in the data store
    /// </summary>
    /// <returns>All countries from the table</returns>
    Task<List<Country>> GetAllCountries();

    /// <summary>
    /// Returns a country object based on the given countryId; otherwise, it returns null value
    /// </summary>
    /// <param name="countryId">CountryId to search</param>
    /// <returns>Matching country or null</returns>
    Task<Country?> GetCountryByCountryId(Guid countryId);

    /// <summary>
    /// Returns a country object based on the given countryName; otherwise, it returns null value
    /// </summary>
    /// <param name="countryName">CountryName to search</param>
    /// <returns>Matching country or null</returns>
    Task<Country?> GetCountryByCountryName(string countryName);
}
