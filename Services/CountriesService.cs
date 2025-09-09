using Entities;
using ServiceContracts;
using ServiceContracts.DTO;

namespace Services
{
    public class CountriesService : ICountriesService
    {
        //private field
        private readonly List<Country> _countries;

        //constructor
        public CountriesService()
        {
            _countries = new List<Country>();
        }

        public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
        {
            //Validation: countryAddRequest parameter can't e null
            if (countryAddRequest == null)
            {
                throw new ArgumentNullException(nameof(countryAddRequest));
            }

            //Validation: CountryName can't be null
            if (countryAddRequest.CountryName == null)
            {
                throw new ArgumentException(nameof(countryAddRequest.CountryName));
            }

            //Validation: CountryName can't be duplicate
            if (_countries.Where(temp => temp.CountryName == countryAddRequest.CountryName).Count() > 0)
            {
                throw new ArgumentException("CountryName can't be duplicate");
            }

            Country country = countryAddRequest.ToCountry();

            country.CountryID = Guid.NewGuid();

            _countries.Add(country);

            return country.ToCountryResponse();
        }

        public List<CountryResponse> GetAllCountries()
        {
            return _countries.Select(temp => temp.ToCountryResponse()).ToList();
        }

        public CountryResponse? GetCountryByCountryID(Guid? countryID)
        {
            //Check if "countryID" is not null.
            if (countryID == null)
            {
                return null;
            }

            Country? country_response_from_list = _countries.FirstOrDefault(temp => temp.CountryID == countryID);

            if (country_response_from_list == null)
            { return null; }

            return country_response_from_list.ToCountryResponse();
        }
    }
}
