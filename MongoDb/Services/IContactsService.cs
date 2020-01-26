using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;

namespace MongoDb.Services
{
    public interface IContactsService
    {
        /// <summary>
        ///     Return all
        /// </summary>
        Task<IList<ContactDto>> GetAll();

        /// <summary>
        ///     Return by id
        /// </summary>
        /// <param name="id"></param>
        Task<ContactDto> GetById(string id);

        /// <summary>
        ///     Add new
        /// </summary>
        /// <param name="contactDto"></param>
        Task<ContactDto> Add(ContactDto contactDto);

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="contactDto"></param>
        Task<ContactDto> Update(ContactDto contactDto);

        /// <summary>
        ///     Delete
        /// </summary>
        /// <param name="id"></param>
        Task<bool> DeleteById(string id);
    }
}