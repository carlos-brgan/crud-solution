using System;
using Xunit;
using System.Collections.Generic;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using ServiceContracts.Enums;
using Xunit.Abstractions;

namespace CRUDTests
{
    public class PersonsServiceTest
    {
        private readonly IPersonsService _personService;
        private readonly ICountriesService _countriesService;
        private readonly ITestOutputHelper _testOutputHelper;

        //constructor
        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {
            _personService = new PersonsService();
            _countriesService = new CountriesService();
            _testOutputHelper = testOutputHelper;
        }

        #region AddPerson

        //When we supply null values as PersonAddRequest it should throw ArgumentNullException
        [Fact]
        public void AddPerson_NullPerson()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }


        //When we supply null value as PersonName, it should throw ArgumentException
        [Fact]
        public void AddPerson_PersonNameIsNull()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _personService.AddPerson(personAddRequest);
            });
        }

        //When we supply proper person details, it should insert the person into the persons list and it should return an object of PersonResponse, which includes the newly generated person id
        [Fact]
        public void AddPerson_ProperPersonDetails()
        {
            //Arrange
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name",
                Email = "peson@example.com",
                Address = "Test address",
                CountryID = Guid.NewGuid(),
                Gender = ServiceContracts.Enums.GenderOptions.Male,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                ReceiveNewsLetters = true
            };

            //Act
            PersonResponse person_response_from_add = _personService.AddPerson(personAddRequest);
            List<PersonResponse> persons_list = _personService.GetAllPersons();

            //Assert
            Assert.True(person_response_from_add.PersonID != Guid.Empty);

            Assert.Contains(person_response_from_add, persons_list);
        }




        #endregion


        #region GetPersonPersonID

        //If we supply null as PersonID, it should return null as PersonResponse

        [Fact]
        public void GetPersonByPersonID_NullPersonID()
        {
            //Arrange
            Guid? personID = null;

            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(personID);

            //Assert
            Assert.Null(person_response_from_get);
        }


        //If we supply a valid person id, it should return the valid person details as PersonResponse object
        [Fact]
        public void GetPersonByPersonID_WithPersonID()
        {
            //Arrange
            CountryAddRequest countryAddRequest = new CountryAddRequest()
            {
                CountryName = "India"
            };

            CountryResponse country_response = _countriesService.AddCountry(countryAddRequest);

            //Act
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Person name",
                Email = "email@sample.com",
                Address = "Test address",
                CountryID = country_response.CountryID,
                DateOfBirth = DateTime.Parse("2000-01-01"),
                Gender = GenderOptions.Male,
                ReceiveNewsLetters = false
            };

            PersonResponse? person_response_from_add = _personService.AddPerson(personAddRequest);

            PersonResponse? person_response_from_get = _personService.GetPersonByPersonID(person_response_from_add.PersonID);


            //Assert
            Assert.Equal(person_response_from_add, person_response_from_get);







        }
        #endregion

        #region GetAllPersons

        //The GetAllPersons() should return an empty list by default
        [Fact]
        public void GetAllPersons_EmptyList()
        {
            //Act
            List<PersonResponse> persons_from_get = _personService.GetAllPersons();

            //Assert
            Assert.Empty(persons_from_get);
        }

        //First, we will add few persons; and then when we call GetAllPersons(), it should return the same persons that were added
        [Fact]
        public void GetAllPersons_AddFewPersons()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "China"
            };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);


            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of smith",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of mary",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Raman",
                Email = "raman@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of raman",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }

            //Act
            List<PersonResponse> persons_list_from_get = _personService.GetAllPersons();

            //Print person_response_list_from_add
            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_from_get in persons_list_from_get)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }


            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_get);
            }

        }

        #endregion


        #region GetFilteredPersons
        //If the search text is empty and search by is"PersonName", it should return all persons
        [Fact]
        public void GetFilteredPersons_EmptySearchText()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "China"
            };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of smith",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of mary",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Raman",
                Email = "raman@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of raman",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
            }

            //Assert
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                Assert.Contains(person_response_from_add, persons_list_from_search);
            }

        }

        //Add few persons and then search based on person name; it should return the matching persons
        [Fact]
        public void GetFilteredPersons_SearchByPersonName()
        {
            //Arrange
            CountryAddRequest country_request_1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryAddRequest country_request_2 = new CountryAddRequest()
            {
                CountryName = "China"
            };

            CountryResponse country_response_1 = _countriesService.AddCountry(country_request_1);
            CountryResponse country_response_2 = _countriesService.AddCountry(country_request_2);

            PersonAddRequest person_request_1 = new PersonAddRequest()
            {
                PersonName = "Smith",
                Email = "smith@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of smith",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_2 = new PersonAddRequest()
            {
                PersonName = "Mary",
                Email = "mary@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of mary",
                CountryID = country_response_1.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            PersonAddRequest person_request_3 = new PersonAddRequest()
            {
                PersonName = "Raman",
                Email = "raman@example.com",
                Gender = GenderOptions.Male,
                Address = "Address of raman",
                CountryID = country_response_2.CountryID,
                DateOfBirth = DateTime.Parse("200-05-06"),
                ReceiveNewsLetters = true
            };

            List<PersonAddRequest> person_requests = new List<PersonAddRequest>()
            {
                person_request_1, person_request_2, person_request_3
            };

            List<PersonResponse> person_response_list_from_add = new List<PersonResponse>();

            foreach (PersonAddRequest person_request in person_requests)
            {
                PersonResponse person_response = _personService.AddPerson(person_request);

                person_response_list_from_add.Add(person_response);
            }

            _testOutputHelper.WriteLine("Expected:");
            foreach (PersonResponse person_response_from_add in person_response_list_from_add)
            {
                _testOutputHelper.WriteLine(person_response_from_add.ToString());
            }


            //Act
            List<PersonResponse> persons_list_from_search = _personService.GetFilteredPersons(nameof(Person.PersonName), "ma");

            _testOutputHelper.WriteLine("Actual:");
            foreach (PersonResponse person_from_get in persons_list_from_search)
            {
                _testOutputHelper.WriteLine(person_from_get.ToString());
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

        }

        #endregion
    }
}
