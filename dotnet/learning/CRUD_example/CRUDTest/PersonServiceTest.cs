using System.Linq.Expressions;
using AutoFixture;
using Entities;
using FluentAssertions;
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
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personsRepository;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IFixture _fixture;

        //constructor
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personsRepository = _personsRepositoryMock.Object;
            _personsService = new PersonsService(_personsRepository);
            _testOutputHelper = testOutputHelper;
            _fixture = new Fixture();
        }


        #region AddPerson

        //When we supply null value as PersonAddRequest it should throw argument null exception
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArgumentNullException()
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
        public async Task AddPerson_PersonNameIsNull_ToBeArgumentException()
        {
            // Given
            PersonAddRequest? personAddRequest = _fixture.Build<PersonAddRequest>()
            .With(temp => temp.PersonName, null as string)
            .With(temp => temp.Gender, GenderOptions.Male)
            .Create();

            Person person = personAddRequest.ToPerson();

            //When PersonsRepository.AddPerson is called, it has to return the same "person" object
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);

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
        public async Task GetPersonByPersonId_NullPersonId_ToBeNull()
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
        public async Task GetPersonByPersonId_withPersonId_ToBeSuccessful()
        {
            //Arrange
            Person person = _fixture.Build<Person>().With(temp => temp.Email, "demoemail@gmail.com")
            .With(temp => temp.country, null as Country)
           .Create();

            PersonResponse person_response_expected = person.ToPersonResponse();

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            PersonResponse? person_response_from_get = await _personsService.GetPersonByPersonId(person.PersonId);


            //Assert
            person_response_from_get.Should().Be(person_response_expected);

        }
        #endregion
        #region GetAllPerson
        //The GetAllPerson() should return an empty list by default
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Arrange
            var persons = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);
            //Act
            List<PersonResponse> persons_from_get = await _personsService.GetAllPerson();

            //Assert
            // Assert.Empty(persons_from_get);
            persons_from_get.Should().BeEmpty();
        }

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public async Task GetAllPersons_WithFewPersons_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
           {
            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create()
           };

            List<PersonResponse> person_response_list_from_add = persons.Select(temp => temp.ToPersonResponse()).ToList();

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);


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
        public async Task GetAllFilteredPersons_EmptySearchText_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
           {
            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create()
           };

            List<PersonResponse> person_response_list_from_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);


            //Act
            List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_expected);
        }
        //First we will add few persons; and then we will search based on person name with some search string. I shoudld return the matching pesons
        [Fact]
        public async Task GetAllFilteredPersons_SearchByPersonName_ToBeSuccessful()
        {
            //Arrange
            List<Person> persons = new List<Person>()
           {
            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create()
           };

            List<PersonResponse> person_response_list_from_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();


            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_expected)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);


            //Act
            List<PersonResponse> persons_list_from_search = await _personsService.GetFilteredPersons(nameof(Person.PersonName), "sa");

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            persons_list_from_search.Should().BeEquivalentTo(person_response_list_from_expected);

        }
        #endregion

        //When we sort based on PersonName in DESC, it should return persons list in descending on PersonName
        #region GetSortedPersons
        [Fact]
        public async Task GetSortedPerson_ToBeSuccessful()
        {
            List<Person> persons = new List<Person>()
           {
            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail2@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create(),

            _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail3@gmail.com")
            .With(temp => temp.country, null as Country)
            .Create()
           };

            List<PersonResponse> person_response_list_from_expected = persons.Select(temp => temp.ToPersonResponse()).ToList();

            _personsRepositoryMock.Setup(temp => temp.GetAllPersons()).ReturnsAsync(persons);


            //Act
            List<PersonResponse> allPersons = await _personsService.GetAllPerson();

            List<PersonResponse> persons_list_from_sort = await _personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_sort)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }

            persons_list_from_sort.Should().BeInDescendingOrder(temp => temp.PersonName);
        }

        #endregion

        #region UpdatePerson
        //When we supply null as PersonUpdateRequest, it should throw ArgumentNullException
        [Fact]
        public async Task UpdatePerson_NullPerson_ToBeArgumentNullException()
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
        public async Task UpdatePerson_InvalidPersonID_ToBeArgumentException()
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
        public async Task UpdatePerson_PersonNameIsNull_ToBeArgumentException()
        {
            //Arrange
            //Create a Person object and Add the object into service
            Person person = _fixture.Build<Person>()
            .With(temp => temp.PersonName, null as string)
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.Gender, "Male")
            .With(temp => temp.country, null as Country)
            .Create();

            PersonResponse person_response_from_add = person.ToPersonResponse();

            PersonUpdateRequest person_update_request = person_response_from_add.ToPersonUpdateRequest();

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
        public async Task UpdatePerson_PersonFullDetailsUpdation_ToBeSuccessful()
        {
            //Arrange
            //Create a Person object and Add the object into service
            Person person = _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.country, null as Country)
            .With(temp => temp.Gender, "Male")
            .Create();

            PersonResponse person_response_from_expected = person.ToPersonResponse();

            PersonUpdateRequest person_update_request = person_response_from_expected.ToPersonUpdateRequest();

            _personsRepositoryMock.Setup(temp => temp.UpdatePerson(It.IsAny<Person>())).ReturnsAsync(person);

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);


            //Act
            PersonResponse person_response_from_update = await _personsService.UpdatePerson(person_update_request);


            //Assert
            // Assert.Equal(person_response_from_get, person_response_from_update);
            person_response_from_update.Should().Be(person_response_from_expected);
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

            //Create a Person object and Add the object into service
            Person person = _fixture.Build<Person>()
            .With(temp => temp.Email, "demoemail1@gmail.com")
            .With(temp => temp.country, null as Country)
            .With(temp => temp.Gender, "Male")
            .Create();

            _personsRepositoryMock.Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(true);

            _personsRepositoryMock.Setup(temp => temp.GetPersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            bool isDeleted = await _personsService.DeletePerson(person.PersonId);

            //Assert
            // Assert.True(isDeleted);
            isDeleted.Should().BeTrue();
        }
        #endregion
    }
}