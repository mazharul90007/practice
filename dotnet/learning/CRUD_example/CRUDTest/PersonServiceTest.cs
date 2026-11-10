using System.Threading.Tasks;
using AutoFixture;
using Entities;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTest
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personsService;
        private readonly ICountriesService _countriesService;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personsRepository;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //constructor
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            //=======Initial Data==========
            var countriesInitialData = new List<Country>() { };
            var personsInitialData = new List<Person>() { };

            //========Mock the DbContext===========
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            //mock ApplicationDbContext instance
            ApplicationDbContext dbContext = dbContextMock.Object;

            //mock DbSet
            dbContextMock.CreateDbSetMock(temp => temp.Countries, countriesInitialData);
            dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            //mock repository must be created before it is passed to the service
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;

            _countriesService = new CountriesService(null);
            _personsService = new PersonsService(_personsRepository, _countriesService);
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
        }


        #region AddPerson

        //When we supply null value as PersonAddRequest it should throw argument null exception
        [Fact]
        public async Task AddPerson_NullPerson()
        {
            // Given
            PersonAddRequest? personAddRequest = null;

            // When

            // Then
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public async Task AddPerson_PersonNameNull()
        {
            // Given
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.PersonName, null as string).Create();

            // When

            // Then
            Func<Task> action = async () =>
            {
                await _personsService.AddPerson(personAddRequest);
            };

            await action.Should().ThrowAsync<ArgumentException>();

        }

        //When we supply proper Person values, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public async Task AddPerson_FullersonDetails_ToBeSuccessful()
        {
            // Given
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "demoemail@gmail.com").Create();

            Person person = personAddRequest.ToPerson();
            PersonResponse person_response_expected = person.ToPersonResponse();

            //If we supply any argument value to the AddPerson method, it should return the same return value
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

            // When
            PersonResponse person_response_from_add = await _personsService.AddPerson(personAddRequest);
            person_response_expected.PersonId = person_response_from_add.PersonId;

            // Then
            // Assert.True(person_response_from_add.PersonId != Guid.Empty);
            person_response_from_add.Should().NotBe(Guid.Empty);

            person_response_from_add.Should().Be(person_response_expected);

        }
        #endregion

        #region GetPersonByPersonId
        //If we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public async Task GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act
            PersonResponse? personResponse_from_get = await _personsService.GetPersonByPersonId(personId);

            //Assert
            // Assert.Null(personResponse_from_get);
            personResponse_from_get.Should().BeNull();

        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public async Task GetPersonByPersonId_withPersonId()
        {
            //Arrange
            CountryAddRequest country_request = _fixture.Create<CountryAddRequest>();

            CountryResponse country_response = await _countriesService.AddCountry(country_request);

            //Act
            PersonAddRequest person_request = _fixture.Build<PersonAddRequest>().With(temp => temp.Email, "demoemail@gmail.com")
            .With(temp => temp.CountryId, country_response.CountryId)
            .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_request);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(person_response_from_add.PersonId);


            //Assert
            // Assert.Equal(person_response_from_add, person_response_from_get);
            person_response_from_get.Should().Be(person_response_from_add);

        }
        #endregion
        #region GetAllPerson
        //The GetAllPerson() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = await _personsService.GetAllPerson();

            //Assert
            // Assert.Empty(persons_from_get);
            persons_from_get.Should().BeEmpty();
        }

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest? country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest? country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.CountryId, country1.CountryId)
            .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_get = await _personsService.GetAllPerson();

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            // foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            // {
            //     Assert.Contains(person_response_from_add, persons_list_from_get);
            // }

            persons_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }


        #endregion

        #region GetFilteredPerson
        //If the search text is empty and search by is "PersonName", it should return all persons

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllFilteredPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest? country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest? country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.CountryId, country1.CountryId)
            .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();


            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_get = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            // foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            // {
            //     Assert.Contains(person_response_from_add, persons_list_from_get);
            // }
            persons_list_from_get.Should().BeEquivalentTo(person_response_list_from_add);
        }
        //First we will add few persons; and then we will search based on person name with some search string. I shoudld return the matching pesons
        [Fact]
        public async Task GetAllFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest? country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest? country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.PersonName, "Mary")
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.CountryId, country1.CountryId)
            .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.PersonName, "Mahir")
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.PersonName, "Rahim")
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();



            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            // foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            // {
            //     if (person_response_from_add.PersonName != null)
            //     {
            //         if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
            //         {
            //             Assert.Contains(person_response_from_add, persons_list_from_search);
            //         }
            //     }
            // }

            persons_list_from_search.Should().HaveCount(2);
            persons_list_from_search.Should().OnlyContain(temp => temp.PersonName!.Contains("ma", StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPerson()
        {
            //Arrange
            CountryAddRequest? country_request_1 = _fixture.Create<CountryAddRequest>();
            CountryAddRequest? country_request_2 = _fixture.Create<CountryAddRequest>();

            CountryResponse country1 = await _countriesService.AddCountry(country_request_1);
            CountryResponse country2 = await _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.CountryId, country1.CountryId)
            .Create();

            PersonAddRequest person_request_2 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();

            PersonAddRequest person_request_3 = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.CountryId, country2.CountryId)
            .Create();


            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = await _personsService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> allPersons = await _personsService.GetAllPerson();
            List<PersonResponse> persons_list_from_sort = await _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }

            // person_response_list_from_add = person_response_list_from_add.OrderByDescending(p => p.PersonName).ToList();


            //Assert
            // for (int i = 0; i < person_response_list_from_add.Count; i++)
            // {
            //     Assert.Equal(person_response_list_from_add[i], persons_list_from_sort[i]);
            // }

            // persons_list_from_sort.Should().BeEquivalentTo(person_response_list_from_add);

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }

        #endregion

        #region UpdatePerson
        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = null;

            //Act
            Func<Task> action = async () =>
            {
                await _personsService.UpdatePerson(person_update_request);
            };

            //Assert
            await action.Should().ThrowAsync<ArgumentNullException>();

        }

        //When we supply invalid person id, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_InvalidPersonID()
        {
            //Arrange
            PersonUpdateRequest? person_update_request = _fixture.Build<PersonUpdateRequest>().Create();


            Func<Task> action = async () =>
            {
                await _personsService.UpdatePerson(person_update_request);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        //When PersonName is null, it should throw ArgumentException
        [Fact]
        public async Task UpdatePerson_PersonNameIsNull()
        {
            //Arrange

            //Create a Country object and add the object into service
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //Create a Person object and Add the object into service
            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.CountryId, country_response_from_add.CountryId)
            .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

            person_update_request.PersonName = null;

            //Act
            Func<Task> action = async () =>
            {
                await _personsService.UpdatePerson(person_update_request);
            };
            //Assert
            await action.Should().ThrowAsync<ArgumentException>();

        }

        //First, add a new person and try to update the person name and email
        [Fact]
        public async Task UpdatePerson_PersonFullDetailsUpdation()
        {
            //Arrange

            //Create a Country object and add the object into service
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //Create a Person object and Add the object into service
            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.CountryId, country_response_from_add.CountryId)
            .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

            person_add_request.PersonName = "Mazharul";
            person_add_request.Email = "updateemail@gmail.com";

            //Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);

            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(person_response_from_update.PersonId);

            //Assert
            // Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_from_get);
        }


        #endregion

        #region DeletePerson
        //If you supply an invalid personId, It should return false
        [Fact]
        public async Task DeletePerson_InvalidPersonId()
        {
            //Act
            bool isDeleted = await _personsService.DeletePerson(Guid.NewGuid());

            //Assert
            // Assert.False(isDeleted);
            isDeleted.Should().BeFalse();
        }

        //If you supply an valid personId, It should return true
        [Fact]
        public async Task DeletePerson_ValidPersonId()
        {
            //Arrange
            //Create a Country object and add the object into service
            CountryAddRequest country_add_request = _fixture.Create<CountryAddRequest>();
            CountryResponse country_response_from_add = await _countriesService.AddCountry(country_add_request);

            //Create a Person object and Add the object into service
            PersonAddRequest person_add_request = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.CountryId, country_response_from_add.CountryId)
            .Create();

            PersonResponse person_response_from_add = await _personsService.AddPerson(person_add_request);

            //Act
            bool isDeleted = await _personsService.DeletePerson(person_response_from_add.PersonId);

            //Assert
            // Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }
        #endregion
    }
}