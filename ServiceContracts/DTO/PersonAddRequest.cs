using Entities;
using ServiceContracts.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// Acts as a DTO for inserting a new person
    /// </summary>
    public class PersonAddRequest
    {
        [Required(ErrorMessage = "Person name cannot be blank")]
        public string? PersonName { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress(ErrorMessage = "Invalid email address format")]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public GenderOptions? Gender { get; set; }
        public Guid? CountryID { get; set; }
        public bool? ReceiveNewsLetters { get; set; }
        public string? Address { get; set; }


        /// <summary>
        /// Converts the current object of PersonAddRequest into a new object of Person
        /// type PersonResponse
        /// </summary>
        /// <returns></returns>
        public Person ToPerson()
        {
            return new Person()
            {
                Address = Address,
                PersonName = PersonName,
                Email = Email,
                DateOfBirth = DateOfBirth,
                Gender = Gender.ToString(),
                CountryID = CountryID,
                ReceiveNewsLetters = ReceiveNewsLetters
            };
        }
    }
}
