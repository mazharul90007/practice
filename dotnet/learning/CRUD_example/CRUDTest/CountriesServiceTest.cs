using System.Threading.Tasks;
using Entities;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;

namespace CRUDTest
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;

        public CountriesServiceTest()
        {
            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesService = new CountriesService(_countriesRepositoryMock.Object);
        }

        #region AddCountry() method tests
        //When CountryAddRequest is null, then AddCountry() should throw ArgumentNullException\
        [Fact]
        public void AddCountry_NullCountry()
        {
            //arrange
            CountryAddRequest? request = null;



            //assert
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //act
                await _countriesService.AddCountry(request);
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
            Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //act
                await _countriesService.AddCountry(request);
            });

        }

        //When the CountryName is duplicate, then AddCountry() should throw ArgumentException
        [Fact]
        public async Task AddCountry_DuplicateCountryName()
        {
            //arrange
            CountryAddRequest request = new CountryAddRequest() { CountryName = "Bangladesh" };

            //repository already has a country with this name
            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
                .ReturnsAsync(new Country() { CountryId = Guid.NewGuid(), CountryName = "Bangladesh" });

            //assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //act
                await _countriesService.AddCountry(request);
            });
        }

        //When you supply proper CountryName, then AddCountry() should return CountryResponse object with proper CountryId and CountryName

        [Fact]
        public async Task AddCountry_ProperCountryDetails()
        {
            //arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Australia"
            };

            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryName(It.IsAny<string>()))
                .ReturnsAsync(null as Country);
            _countriesRepositoryMock
                .Setup(temp => temp.AddCountry(It.IsAny<Country>()))
                .ReturnsAsync((Country c) => c);

            //act
            CountryResponse response = await _countriesService.AddCountry(request);

            //assert
            Assert.NotNull(response);
            Assert.True(response.CountryId != Guid.Empty);
            Assert.Equal("Australia", response.CountryName);
            _countriesRepositoryMock.Verify(temp => temp.AddCountry(It.IsAny<Country>()), Times.Once);
        }

        #endregion

        #region GetAllCountries() method tests
        [Fact]
        public async Task GetAllCountries_EmptyList()
        {
            _countriesRepositoryMock
                .Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(new List<Country>());

            //act
            List<CountryResponse> response = await _countriesService.GetAllCountries();

            //assert
            Assert.NotNull(response);
            Assert.Empty(response);
        }

        [Fact]
        public async Task GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<Country> countries = new List<Country>()
            {
                new Country(){CountryId = Guid.NewGuid(), CountryName = "Bangladesh"},
                new Country(){CountryId = Guid.NewGuid(), CountryName = "India"},
                new Country(){CountryId = Guid.NewGuid(), CountryName = "Nepal"}
            };
            List<CountryResponse> expectedList = countries.Select(temp => temp.ToCountryResponse()).ToList();

            _countriesRepositoryMock
                .Setup(temp => temp.GetAllCountries())
                .ReturnsAsync(countries);

            //Act
            List<CountryResponse> actualCountryResponseList = await _countriesService.GetAllCountries();

            //Assert
            foreach (CountryResponse expected_country in expectedList)
            {
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }

        #endregion

        #region GetCountryByCountryId
        [Fact]
        //If we supply null as CountryId, it should return null as CountryResponse
        public async Task GetCountryByCountryId_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse? country_response_from_get_method = await _countriesService.GetCountryByCountryId(countryId);

            //Assert
            Assert.Null(country_response_from_get_method);
        }

        [Fact]
        //If we supply a valid countryId, it should return the matching country details as CountryResponse object
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            //Arrange
            Country country = new Country() { CountryId = Guid.NewGuid(), CountryName = "China" };
            CountryResponse expected = country.ToCountryResponse();

            _countriesRepositoryMock
                .Setup(temp => temp.GetCountryByCountryId(It.IsAny<Guid>()))
                .ReturnsAsync(country);

            //Act
            CountryResponse? country_response_from_get = await _countriesService.GetCountryByCountryId(country.CountryId);

            //Assert
            Assert.Equal(expected, country_response_from_get);
        }

        #endregion

    }
}