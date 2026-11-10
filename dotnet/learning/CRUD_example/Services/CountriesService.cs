using ServiceContracts;
using ServiceContracts.DTO;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Services;

public class CountriesService : ICountriesService
{
    //private field
    private readonly ICountriesRepository _countriesRepository;

    //constructor
    public CountriesService(ICountriesRepository countriesRepository)
    {
        _countriesRepository = countriesRepository;
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
        if (await _countriesRepository.GetCountryByCountryName(countryAddRequest.CountryName) != null)
        {

            throw new ArgumentException("Country name can't be duplicate", nameof(countryAddRequest.CountryName));
        }

        //============Implementation=============

        //Convert object from CountryAddRequest to Country type
        Country country = countryAddRequest.ToCountry();

        //generate CountryId
        country.CountryId = Guid.NewGuid();

        //Add country object into _countriesRepository
        await _countriesRepository.AddCountry(country);

        return country.ToCountryResponse();
    }


    //===============Get All Countries=================

    public async Task<List<CountryResponse>> GetAllCountries()
    {
        List<Country> countries = await _countriesRepository.GetAllCountries();
        return countries.Select(country => country.ToCountryResponse()).ToList();
    }

    //===============Get Country by Id==================
    public async Task<CountryResponse?> GetCountryByCountryId(Guid? countryId)
    {
        if (countryId == null)
        {
            return null;
        }

        Country? country_response_from_list = await _countriesRepository.GetCountryByCountryId(countryId.Value);

        if (country_response_from_list == null)
        {
            return null;
        }

        return country_response_from_list.ToCountryResponse();

    }

    //===============Upload Countries from Excel File==================
    public async Task<int> UploadCountriesFromExcelFile(IFormFile formFile)
    {
        MemoryStream memoryStream = new MemoryStream();
        await formFile.CopyToAsync(memoryStream);
        int countriesInserted = 0;

        using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
        {
            ExcelWorksheet? workSheet = excelPackage.Workbook.Worksheets["Countries"];
            if (workSheet == null || workSheet.Dimension == null)
            {
                return 0;
            }

            int rowCount = workSheet.Dimension.Rows;

            //row 1 is the header, so data starts from row 2
            for (int row = 2; row <= rowCount; row++)
            {
                string? cellValue = Convert.ToString(workSheet.Cells[row, 1].Value)?.Trim();

                if (!string.IsNullOrEmpty(cellValue))
                {
                    string countryName = cellValue;

                    //skip duplicate countries
                    if (await _countriesRepository.GetCountryByCountryName(countryName) == null)
                    {
                        Country country = new Country() { CountryId = Guid.NewGuid(), CountryName = countryName };
                        await _countriesRepository.AddCountry(country);
                        countriesInserted++;
                    }
                }
            }
        }
        return countriesInserted;
    }
}
