using System;
using ServiceContracts.DTO;


namespace ServiceContracts
{
    /// <summary>
    /// Represents business logic for manipulating Person entity
    /// </summary>
    public interface IPersonsService
    {
        /// <summary>
        /// Adds a new person into the list of persons
        /// </summary>
        /// <param name="personAddRequest"></param>
        /// <returns>Return the same person details, along with newly generated PersonID</returns>
        PersonResponse AddPerson(PersonAddRequest? personAddRequest);

        /// <summary>
        /// Returns all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns>
        List<PersonResponse> GetAllPersons();

        /// <summary>
        /// Returns the person object based on the given person id
        /// </summary>
        /// <param name="personID"></param>
        PersonResponse GetPersonByPersonID(Guid? personID);


        /// <summary>
        /// Retrieves a list of persons filtered based on the specified search criteria.
        /// </summary>
        /// <param name="searchBy">The field to filter by. Valid values may include "Name", "City", or other supported fields. If null or
        /// empty, no specific field is used for filtering.</param>
        /// <param name="searchString">The value to search for within the specified field. If null or empty, no filtering is applied.</param>
        /// <returns>A list of <see cref="PersonResponse"/> objects that match the specified search criteria. Returns an empty
        /// list if no matches are found.</returns>
        public List<PersonResponse> GetFilteredPersons(string? searchBy, string? searchString);
    }
}
