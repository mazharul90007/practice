
using System.ComponentModel.DataAnnotations;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonService
    {
        //private field
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        //constructor
        public PersonsService(bool initialize = true)
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();

            if (initialize)
            {
                _persons.AddRange(new List<Person>
                {
                    new Person
                    {
                        PersonId = Guid.Parse("6a60b993-b97b-4b61-bd5b-37ca1a7c4803"),
                        PersonName = "John Smith",
                        Email = "john.smith@example.com",
                        DateOfBirth = DateTime.Parse("1990-05-14"),
                        Gender = GenderOptions.Male.ToString(),
                        CountryId = Guid.Parse("d65be9ce-46d1-4f8b-b132-bd0a024ba436"),
                        Address = "12 Main Street, New York",
                        ReceiveNewsLetters = true
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("c198299e-77db-46cc-848a-ce21fdfac1fe"),
                        PersonName = "Emma Wilson",
                        Email = "emma.wilson@example.com",
                        DateOfBirth = DateTime.Parse("1993-08-22"),
                        Gender = GenderOptions.Female.ToString(),
                        CountryId = Guid.Parse("647b7454-94a5-4e7f-a83e-02d6ec920f82"),
                        Address = "45 Maple Avenue, Toronto",
                        ReceiveNewsLetters = false
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("5567169a-ec42-4281-b995-ee2988722c3e"),
                        PersonName = "Oliver Brown",
                        Email = "oliver.brown@example.com",
                        DateOfBirth = DateTime.Parse("1988-01-30"),
                        Gender = GenderOptions.Male.ToString(),
                        CountryId = Guid.Parse("d67e5387-0d35-4784-b7c5-6f77b1a59536"),
                        Address = "7 Baker Street, London",
                        ReceiveNewsLetters = true
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("02a8296a-33d6-434b-baf9-1ae99423baf4"),
                        PersonName = "Priya Sharma",
                        Email = "priya.sharma@example.com",
                        DateOfBirth = DateTime.Parse("1995-11-09"),
                        Gender = GenderOptions.Female.ToString(),
                        CountryId = Guid.Parse("f06919f2-43c6-4255-8523-4d1bc30a01b6"),
                        Address = "21 MG Road, Bengaluru",
                        ReceiveNewsLetters = true
                    },
                    new Person
                    {
                        PersonId = Guid.Parse("48eae510-3573-42c1-9133-ab1bca68d04e"),
                        PersonName = "Liam Taylor",
                        Email = "liam.taylor@example.com",
                        DateOfBirth = DateTime.Parse("1991-03-18"),
                        Gender = GenderOptions.Male.ToString(),
                        CountryId = Guid.Parse("2ed188ec-0464-4a99-b723-996b2283d4c6"),
                        Address = "88 Beach Road, Sydney",
                        ReceiveNewsLetters = false
                    }
                });
            }
        }

        private PersonResponse ConvertPersonToPersonResponse(Person person)
        {
            //convert the Person object into PersonResponse type
            PersonResponse personResponse = person.ToPersonResponse();

            personResponse.Country = _countriesService.GetCountryByCountryId(person.CountryId)?.CountryName;
            return personResponse;
        }

        //=================Add a person==================
        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //==========Model validation============
            ValidationHelper.ModelValidation(personAddRequest);

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //generate PersonId
            person.PersonId = Guid.NewGuid();

            //add the Person object to person list
            _persons.Add(person);

            //convert the Person object into PersonResponse type
            return ConvertPersonToPersonResponse(person);


        }

        //==============Get all persons===============
        public List<PersonResponse> GetAllPerson()
        {
            return _persons.Select(p => ConvertPersonToPersonResponse(p)).ToList();
        }

        //==============Get person by Id================
        public PersonResponse? GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

            Person? person = _persons.FirstOrDefault(c => c.PersonId == personId);

            if (person == null)
            {
                return null;
            }

            return ConvertPersonToPersonResponse(person);
        }

        //===============Search persons by search key===============
        public List<PersonResponse> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = GetAllPerson();
            List<PersonResponse> matchingPersons = allPersons;

            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return matchingPersons;
            }

            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):
                    matchingPersons = allPersons.Where(p => !string.IsNullOrEmpty(p.PersonName) ? p.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Email):
                    matchingPersons = allPersons.Where(p => !string.IsNullOrEmpty(p.Email) ? p.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.DateOfBirth):
                    matchingPersons = allPersons.Where(p => (p.DateOfBirth != null) ? p.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Gender):
                    matchingPersons = allPersons.Where(p => !string.IsNullOrEmpty(p.Gender) ? p.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.CountryId):
                    matchingPersons = allPersons.Where(p => !string.IsNullOrEmpty(p.Country) ? p.Country.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                case nameof(PersonResponse.Address):
                    matchingPersons = allPersons.Where(p => !string.IsNullOrEmpty(p.Address) ? p.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true).ToList();
                    break;

                default:
                    matchingPersons = allPersons;
                    break;
            }

            return matchingPersons;
        }

        //==========Sort Persons list by ASC or DESC===========
        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }

            List<PersonResponse> sortedPersons = (sortBy, sortOrder)
                switch
            {
                (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Email, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.DateOfBirth).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Age).ToList(),

                (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Age).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Gender, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Country, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.Address, StringComparer.OrdinalIgnoreCase).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.ASC) => allPersons.OrderBy(p => p.ReceiveNewsLetters).ToList(),

                (nameof(PersonResponse.ReceiveNewsLetters), SortOrderOptions.DESC) => allPersons.OrderByDescending(p => p.ReceiveNewsLetters).ToList(),

                _ => allPersons
            };

            return sortedPersons;

        }

        //Update Person details by PersonId
        public PersonResponse UpdatePerson(PersonUpdateRequest personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //Get matching person object to update
            Person? matchingPerson = _persons.FirstOrDefault(p => p.PersonId == personUpdateRequest.PersonId);

            if (matchingPerson == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }

            //Update all details
            matchingPerson.PersonName = personUpdateRequest.PersonName;
            matchingPerson.Email = personUpdateRequest.Email;
            matchingPerson.DateOfBirth = personUpdateRequest.DateOfBirth;
            matchingPerson.Gender = personUpdateRequest.Gender?.ToString();
            matchingPerson.CountryId = personUpdateRequest.CountryId;
            matchingPerson.Address = personUpdateRequest.Address;
            matchingPerson.ReceiveNewsLetters = personUpdateRequest.ReceiveNewsLetters;

            return ConvertPersonToPersonResponse(matchingPerson);
        }

        //Delete a Person details by PersonId
        public bool DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = _persons.FirstOrDefault(p => p.PersonId == personId);

            if (person == null)
            {
                return false;
            }

            _persons.RemoveAll(p => p.PersonId == personId);
            return true;
        }
    }
}