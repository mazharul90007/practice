using AutoFixture;
using Moq;
using ServiceContracts;

namespace CRUDTest
{
    public class PersonsControllerTest
    {
        private readonly ICountriesService _countriesService;
        private readonly IPersonService _personsService;
        private readonly Mock<ICountriesService> _countriesServiceMock;
        private readonly Mock<IPersonService> _personsServiceMock;
        private readonly Fixture _fixture;

        public PersonsControllerTest()
        {
            _fixture = new Fixture();
            _personsServiceMock = new Mock<IPersonService>();
            _countriesServiceMock = new Mock<ICountriesService>();

            _personsService = _personsServiceMock.Object;
            _countriesService = _countriesServiceMock.Object;
        }

        #region Index
        [Fact]
        public async Task Index_ShouldReturnIndexViewWithPersonList()
        {
            
        }

        #endregion


    }
}