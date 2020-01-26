using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;

namespace MongoDb.Services
{
    public interface ICustomFieldsService
    {
        /// <summary>
        ///     Return all
        /// </summary>
        Task<IList<CustomFieldDto>> GetAll();

        /// <summary>
        ///     Return by id
        /// </summary>
        /// <param name="id"></param>
        Task<CustomFieldDto> GetById(string id);

        /// <summary>
        ///     Check if customField exists
        /// </summary>
        /// <param name="name"></param>
        Task<bool> CustomFieldExists(string name);

        /// <summary>
        ///     Add new
        /// </summary>
        /// <param name="customFieldDto"></param>
        Task<CustomFieldDto> Add(CustomFieldDto customFieldDto);

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="customFieldDto"></param>
        Task<CustomFieldDto> Update(CustomFieldDto customFieldDto);

        /// <summary>
        ///     Delete
        /// </summary>
        /// <param name="id"></param>
        Task<bool> DeleteById(string id);
    }
}