using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;
using Xunit.Abstractions;

namespace CRUDTest
{
    public class PersonServiceTest
    {
        private readonly IPersonService _personService;
        private readonly ICountriesService _countryService;
        private readonly ITestOutputHelper _testOutputHelper;

        //constructor
        public PersonServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countryService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }


        #region AddPerson

        //When we supply null value as PersonAddRequest it should throw argument null exception
        [Fact]
        public void AddPerson_NullPerson()
        {
            // Given
            PersonAddRequest? personAddRequest = null;

            // When

            // Then
            Assert.Throws<ArgumentNullException>(() => _personService.AddPerson(personAddRequest));
        }

        //When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameNull()
        {
            // Given
            PersonAddRequest? personAddRequest = new PersonAddRequest() { PersonName = null };

            // When

            // Then
            Assert.Throws<ArgumentException>(() => _personService.AddPerson(personAddRequest));
        }

        //When we supply proper Person values, it should insert the person into the persons list; and it should return an object of PersonResponse, which includes with the newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            // Given
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Mazharul Islam",
                Email = "mazharul@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                CountryId = Guid.NewGuid(),
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };

            // When
            PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personService.GetAllPerson();

            // Then
            Assert.True(person_response_from_add.PersonId != Guid.Empty);
            Assert.Contains(person_response_from_add, persons_list);

        }
        #endregion

        #region GetPersonByPersonId
        //If we supply null as PersonId, it should return null as PersonResponse
        [Fact]
        public void GetPersonByPersonId_NullPersonId()
        {
            //Arrange
            Guid? personId = null;

            //Act
            PersonResponse? personResponse_from_get = _personService.GetPersonByPersonId(personId);

            //Assert
            Assert.Null(personResponse_from_get);

        }

        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonByPersonId_withPersonId()
        {
            //Arrange
            CountryAddRequest country_request = new CountryAddRequest() { CountryName = "Bangladesh" };

            CountryResponse country_response = _countryService.AddCountry(country_request);

            //Act
            PersonAddRequest person_request = new PersonAddRequest()
            {
                PersonName = "Mazharul Islam",
                Email = "mazharul@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                CountryId = country_response.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };

            PersonResponse person_response_from_add = _personService.AddPerson(person_request);

            PersonResponse person_response_from_get = _personService.GetPersonByPersonId(person_response_from_add.PersonId);


            //Assert
            Assert.Equal(person_response_from_add, person_response_from_get);

        }
        #endregion
        #region GetAllPerson
        //The GetAllPerson() should return an empty list by default
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = _personService.GetAllPerson();

            //Assert
            Assert.Empty(persons_from_get);
        }

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest? country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? country_request_2 = new CountryAddRequest() { CountryName = "Canada" };

            CountryResponse country1 = _countryService.AddCountry(country_request_1);
            CountryResponse country2 = _countryService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Mazharul Islam",
                Email = "mazharul@example.com",
                DateOfBirth = DateTime.Parse("1997-05-01"),
                Gender = GenderOptions.Male,
                CountryId = country1.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Moktar Hossain",
                Email = "moktar@example.com",
                DateOfBirth = DateTime.Parse("1984-01-01"),
                Gender = GenderOptions.Male,
                CountryId = country2.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Zara",
                Email = "zara@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Female,
                CountryId = country2.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };



            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_get = _personService.GetAllPerson();

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }
        }


        #endregion

        #region GetFilteredPerson
        //If the search text is empty and search by is "PersonName", it should return all persons

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public void GetAllFilteredPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest? country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? country_request_2 = new CountryAddRequest() { CountryName = "Canada" };

            CountryResponse country1 = _countryService.AddCountry(country_request_1);
            CountryResponse country2 = _countryService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Mazharul Islam",
                Email = "mazharul@example.com",
                DateOfBirth = DateTime.Parse("1997-05-01"),
                Gender = GenderOptions.Male,
                CountryId = country1.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Moktar Hossain",
                Email = "moktar@example.com",
                DateOfBirth = DateTime.Parse("1984-01-01"),
                Gender = GenderOptions.Male,
                CountryId = country2.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Zara",
                Email = "zara@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Female,
                CountryId = country2.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };



            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_get = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }
        }
        //First we will add few persons; and then we will search based on person name with some search string. I shoudld return the matching pesons
        [Fact]
        public void GetAllFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest? country_request_1 = new CountryAddRequest() { CountryName = "USA" };
            CountryAddRequest? country_request_2 = new CountryAddRequest() { CountryName = "Canada" };

            CountryResponse country1 = _countryService.AddCountry(country_request_1);
            CountryResponse country2 = _countryService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Mazharul Islam",
                Email = "mazharul@example.com",
                DateOfBirth = DateTime.Parse("1997-05-01"),
                Gender = GenderOptions.Male,
                CountryId = country1.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };
            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Moktar Hossain",
                Email = "moktar@example.com",
                DateOfBirth = DateTime.Parse("1984-01-01"),
                Gender = GenderOptions.Male,
                CountryId = country2.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Zara",
                Email = "zara@example.com",
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Female,
                CountryId = country2.CountryId,
                Address = "Dhaka, Bangladesh",
                ReceiveNewsLetters = true
            };



            List<PersonAddRequest> person_requests = new List<PersonAddRequest>() { person_request_1, person_request_2, person_request_3 };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected: ");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            //Print person_response_list_from_get
            _testOutputHelper.WriteLine("Actual_result: ");
            foreach (PersonResponse person_list_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_list_from_get.ToString());
            }


            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                if (person_response_from_add.PersonName != null)
                {
                    if (person_response_from_add.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                    {
                        Assert.Contains(person_response_from_add, persons_list_from_search);
                    }
                }
            }

        #endregion

        }
    }
}