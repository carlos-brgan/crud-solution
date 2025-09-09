using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System;
using System.Collections.Generic;


namespace CRUDTests
{
    public class CountriesServiceTest
    {
        private readonly ICountriesService _countriesService;

        //constructor
        public CountriesServiceTest()
        {
            _countriesService = new CountriesService();
        }

        #region AddCountry
        //When CountryAddRequest is null, it should throw ArgumentNullException
        [Fact]
        public void AddCountry_NullCountry()
        {
            //Arrange
            CountryAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });

        }

        //When CountryName is null, it should throw ArgumentException
        [Fact]
        public void AddCountry_CountryNameIsNull()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = null
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request);
            });

        }

        //When CountryName is duplicated, it should throw ArgumentException
        [Fact]
        public void AddCountry_DuplicateCountryName()
        {
            //Arrange
            CountryAddRequest? request1 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            CountryAddRequest? request2 = new CountryAddRequest()
            {
                CountryName = "USA"
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                _countriesService.AddCountry(request1);
                _countriesService.AddCountry(request2);
            });

        }

        //When CountryName is valid, it should insert it into the list of countries
        [Fact]
        public void AddCountry_ProperCountryDetails()
        {
            //Arrange
            CountryAddRequest? request = new CountryAddRequest()
            {
                CountryName = "Japan"
            };

            //Assert
            CountryResponse response = _countriesService.AddCountry(request);

            List<CountryResponse> countries_from_GetAllCountries = _countriesService.GetAllCountries();

            //Act
            Assert.True(response.CountryID != Guid.Empty);
            Assert.Contains(response, countries_from_GetAllCountries);
        }
        #endregion

        #region GetAllCountries

        [Fact]
        //The list of countries should be empty by default
        public void GetAllCountries_EmptyList()
        {
            //Arrange
            //Act
            List<CountryResponse> countries = _countriesService.GetAllCountries();
            //Assert
            Assert.NotNull(countries);
            Assert.True(countries.Count == 0);
            Assert.Empty(countries);
        }

        [Fact]
        public void GetAllCountries_AddFewCountries()
        {
            //Arrange
            List<CountryAddRequest> country_request_list = new List<CountryAddRequest>()
            {
                new CountryAddRequest(){ CountryName="USA"},
                new CountryAddRequest(){ CountryName="UK"},
            };

            //Act
            List<CountryResponse> countries_list_from_add_country = new List<CountryResponse>();


            foreach (CountryAddRequest country_request in country_request_list)
            {
                countries_list_from_add_country.Add(_countriesService.AddCountry(country_request));
            }

            List<CountryResponse> actualCountryResponseList = _countriesService.GetAllCountries();

            //read each element from countries_list_from_add_country
            foreach (CountryResponse expected_country in countries_list_from_add_country)
            {
                //check whether expected_country is present in countries_list_from_get_all_countries
                Assert.Contains(expected_country, actualCountryResponseList);
            }
        }

        #endregion
    }
}
