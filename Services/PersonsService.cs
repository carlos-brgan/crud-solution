using System;
using Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class PersonsService : IPersonsService
    {
        //Private fields
        private readonly List<Person> _persons;
        private readonly ICountriesService _countriesService;

        //Constructor
        public PersonsService()
        {
            _persons = new List<Person>();
            _countriesService = new CountriesService();
        }


        private PersonResponse ConvertPersonIntoPersonResponse(Person person)
        {
            PersonResponse personResponse = person.ToPersonResponse();
            personResponse.Country = _countriesService.GetCountryByCountryID(person.CountryID)?.CountryName;
            return personResponse;
        }

        public PersonResponse AddPerson(PersonAddRequest? personAddRequest)
        {
            //check if PersonAddRequest is not null
            if (personAddRequest == null)
            {
                throw new ArgumentNullException(nameof(personAddRequest));
            }

            //Model validations
            ValidationContext validationContext = new ValidationContext(personAddRequest);

            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(personAddRequest, validationContext, validationResults, true);

            if(!isValid)
            {
                throw new ArgumentException(validationResults.FirstOrDefault()?.ErrorMessage);
            }

            //Validate PersonName is not null
            if (string.IsNullOrEmpty(personAddRequest.PersonName))
            {
                throw new ArgumentException("PersonName can't be blank");
            }

            //convert personAddRequest into Person type
            Person person = personAddRequest.ToPerson();

            //Generate new personID and assign it to person object
            person.PersonID = Guid.NewGuid();

            //add person object to persons list
            _persons.Add(person);

            //convert the newly added person object into PersonResponse type and return it

            return ConvertPersonIntoPersonResponse(person);
        }

        public List<PersonResponse> GetAllPersons()
        {
            throw new NotImplementedException();
        }
    }
}
