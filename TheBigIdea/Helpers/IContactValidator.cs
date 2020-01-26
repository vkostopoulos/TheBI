using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;

namespace TheBigIdea.Helpers
{
    public interface IContactValidator
    {
        /// <summary>
        ///     Validate contact's actions and customfields
        /// </summary>
        /// <param name="contactDto"></param>
        Task<string> ValidateContact(ContactDto contactDto);
    }
}