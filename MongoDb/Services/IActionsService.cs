using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;

namespace MongoDb.Services
{
    public interface IActionsService
    {
        /// <summary>
        ///     Return all
        /// </summary>
        Task<IList<ActionDto>> GetAll();

        /// <summary>
        ///     Return by id
        /// </summary>
        /// <param name="id"></param>
        Task<ActionDto> GetById(string id);

        /// <summary>
        ///     Check if action exists
        /// </summary>
        /// <param name="name"></param>
        Task<bool> ActionExists(string name);

        /// <summary>
        ///     Add new
        /// </summary>
        /// <param name="actionDto"></param>
        Task<ActionDto> Add(ActionDto actionDto);

        /// <summary>
        ///     Update
        /// </summary>
        /// <param name="actionDto"></param>
        Task<ActionDto> Update(ActionDto actionDto);

        /// <summary>
        ///     Delete
        /// </summary>
        /// <param name="id"></param>
        Task<bool> DeleteById(string id);
    }
}