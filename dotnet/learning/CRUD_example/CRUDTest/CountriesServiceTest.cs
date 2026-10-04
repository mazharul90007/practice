using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry() method tests
        //When CountryAddRequest is null, then AddCountry() should throw ArgumentNullException\
        [Fact]
        public void AddCountry_NullCountry()
        {
            //arrange
            CountryAddRequest? request = null;



            //assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //act
                _countriesService.AddCountry(request);
            });

        }

        //When the CountryName is null or empty, then AddCountry() should throw ArgumentException

        [Fact]
        public void AddCountry_NullCountryName()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //assert
            Assert.Throws<ArgumentException>(() =>
            {
                //act
                _countriesService.AddCountry(request);
            });

        }

        //When the CountryName is duplicate, then AddCountry() should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "Bangladesh"
            };

            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "Bangladesh"
            };


            //assert
            Assert.Throws<ArgumentException>(() =>
            {
                //act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });

        }

        //When you supply proper CountryName, then AddCountry() should return CountryResponse object with proper CountryId and CountryName

        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Australia"
            };

            //act
            CountryResponse response = _countriesService.AddCountry(request);

            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //assert
            Assert.NotNull(response);
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Equal("Australia", response.CountryName);
            Assert.Contains(response, countries_from_GetAllCountries);
        }

        #endregion

        #region GetAllCountries() method tests
        [Fact]
        public void GetAllCountries_EmptyList()
        {
            //act
            List<CountryResponse> response = _countriesService.GetAllCountries();

            //assert
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){CountryName = "Bangladesh"},
                new CountryAddRequest(){CountryName = "India"},
                new CountryAddRequest(){CountryName = "Nepal"}
            };

            //Act
            List<CountryResponse> countires_list_from_add_country = new List<CountryResponse>();

            foreach (CountryAddRequest country_request in country_request_list)
            {
                countires_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            //read each element from countries_list_from_add_country
            foreach (CountryResponse expected_country in countires_list_from_add_country)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }

        #endregion

        #region GetCountryByCountryId
        [Fact]
        //If we supply null as CountryId, it should return null as CountryResponse
        public void GetCountryByCountryId_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse? country_response_from_get_method = _countriesService.GetCountryByCountryId(countryId);

            //Assert
            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        //If we supply a valid countryId, it should return the matching country details as CountryResponse object
        public void GetCountryByCountryId_ValidCountryId()
        {
            //Arrange
            CountryAddRequest? country_add_request = new CountryAddRequest() { CountryName = "China" };
            CountryResponse country_response_from_add_request = _countriesService.AddCountry(country_add_request);

            //Act
            CountryResponse? country_response_from_get = _countriesService.GetCountryByCountryId(country_response_from_add_request.CountryId);


            //Assert
            Assert.Equal(country_response_from_add_request, country_response_from_get);

        }

        #endregion

    }
}