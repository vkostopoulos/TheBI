using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;
using MongoDb.Services;

namespace TheBigIdea.Helpers.Impl
{
    public class CustomFieldValidator : ICustomFieldValidator
    {
        private readonly ICustomFieldsService _customFieldsService;

        public CustomFieldValidator(ICustomFieldsService customFieldsService)
        {
            _customFieldsService = customFieldsService;
        }

        public async Task<string> ValidateCustomField(CustomFieldDto customFieldDto)
        {
            if (string.IsNullOrEmpty(customFieldDto.Id))
            {
                // Check customfields
                var result = await _customFieldsService.CustomFieldExists(customFieldDto.Name);
                return result ? $"CustomField with Name #{customFieldDto.Name} already exists!" : string.Empty;
            }
            else
            {
                // Check customfields
                var result = await _customFieldsService.GetById(customFieldDto.Id);
                if (result.Name == customFieldDto.Name) return string.Empty;
                var exists = await _customFieldsService.CustomFieldExists(customFieldDto.Name);
                return exists ? $"CustomField with Name #{customFieldDto.Name} already exists!" : string.Empty;
            }
        }
    }
}