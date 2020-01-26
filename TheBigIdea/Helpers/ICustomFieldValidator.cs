using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;

namespace TheBigIdea.Helpers
{
    public interface ICustomFieldValidator
    {
        /// <summary>
        ///     Validate customfield
        /// </summary>
        /// <param name="customFieldDto"></param>
        Task<string> ValidateCustomField(CustomFieldDto customFieldDto);
    }
}