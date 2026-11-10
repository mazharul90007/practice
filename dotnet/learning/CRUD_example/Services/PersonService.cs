
using System.ComponentModel.DataAnnotations;
using Entities;
using Microsoft.EntityFrameworkCore;
using RepositoryContracts;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services
{
    public class PersonsService : IPersonService
    {
        //private field
        private readonly IPersonsRepository _personsRepository;
        private readonly ICountriesService _countriesService;

        //constructor
        public PersonsService(IPersonsRepository personsDbContext, ICountriesService countriesService)
        {
            _personsRepository = personsDbContext;
            _countriesService = countriesService;
        }



        //=================Add a person==================
        public async Task<PersonResponse> AddPerson(PersonAddRequest? personAddRequest)
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
            await _personsRepository.AddPerson(person);

            //convert the Person object into PersonResponse type
            return person.ToPersonResponse();
        }

        //==============Get all persons===============
        public async Task<List<PersonResponse>> GetAllPerson()
        {
            //SELECT * from Persons
            var persons = await _personsRepository.GetAllPersons();
            return persons.Select(temp => temp.ToPersonResponse()).ToList();

            //=========Stored Procedure==========
            // return _personsRepository.sp_GetAllPersons()
            // .Select(p => ConvertPersonToPersonResponse(p)).ToList();
        }

        //==============Get person by Id================
        public async Task<PersonResponse?> GetPersonByPersonId(Guid? personId)
        {
            if (personId == null)
            {
                return null;
            }

            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);

            if (person == null)
            {
                return null;
            }

            return person.ToPersonResponse();
        }

        //===============Search persons by search key===============
        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            if (string.IsNullOrEmpty(searchBy) || string.IsNullOrEmpty(searchString))
            {
                return await GetAllPerson();
            }

            //Plain Contains() is translated to SQL LIKE; SQL Server is case-insensitive by default
            List<Person> persons = searchBy switch
            {
                nameof(PersonResponse.PersonName) =>
                    await _personsRepository.GetFilteredPersons(temp => temp.PersonName!.Contains(searchString)),

                nameof(PersonResponse.Email) =>
                    await _personsRepository.GetFilteredPersons(temp => temp.Email!.Contains(searchString)),

                //DateTime.ToString("dd MMM yyyy") can't be translated to SQL, so filter this one in memory
                nameof(PersonResponse.DateOfBirth) =>
                    (await _personsRepository.GetAllPersons())
                        .Where(temp => temp.DateOfBirth != null && temp.DateOfBirth.Value.ToString("dd MMM yyyy").Contains(searchString, StringComparison.OrdinalIgnoreCase))
                        .ToList(),

                nameof(PersonResponse.Gender) =>
                    await _personsRepository.GetFilteredPersons(temp => temp.Gender!.Contains(searchString)),

                nameof(PersonResponse.CountryId) =>
                    await _personsRepository.GetFilteredPersons(temp => temp.country!.CountryName!.Contains(searchString)),

                nameof(PersonResponse.Address) =>
                    await _personsRepository.GetFilteredPersons(temp => temp.Address!.Contains(searchString)),

                _ => await _personsRepository.GetAllPersons()
            };

            return persons.Select(temp => temp.ToPersonResponse()).ToList();
        }

        //==========Sort Persons list by ASC or DESC===========
        public async Task<List<PersonResponse>> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrder)
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
        public async Task<PersonResponse> UpdatePerson(PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
            {
                throw new ArgumentNullException(nameof(Person));
            }

            //Validation
            ValidationHelper.ModelValidation(personUpdateRequest);

            //Get matching person object to update
            Person? matchingPerson = await _personsRepository.GetPersonByPersonId(personUpdateRequest.PersonId);

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

            await _personsRepository.UpdatePerson(matchingPerson); //UPDATE
            return matchingPerson.ToPersonResponse();
        }

        //Delete a Person details by PersonId
        public async Task<bool> DeletePerson(Guid? personId)
        {
            if (personId == null)
            {
                throw new ArgumentNullException(nameof(personId));
            }

            Person? person = await _personsRepository.GetPersonByPersonId(personId.Value);

            if (person == null)
            {
                return false;
            }

            await _personsRepository.DeletePersonByPersonId(personId.Value);

            return true;
        }
    }
}