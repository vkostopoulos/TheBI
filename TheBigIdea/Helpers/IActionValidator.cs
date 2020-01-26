using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;

namespace TheBigIdea.Helpers
{
    public interface IActionValidator
    {
        /// <summary>
        ///     Validate action
        /// </summary>
        /// <param name="actionDto"></param>
        Task<string> ValidateAction(ActionDto actionDto);
    }
}