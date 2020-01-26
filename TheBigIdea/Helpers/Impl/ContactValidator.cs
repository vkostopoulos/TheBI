using System.Threading.Tasks;
using MongoDb.Models.Dto;
using MongoDb.Models.Dto.Crm;
using MongoDb.Services;

namespace TheBigIdea.Helpers.Impl
{
    public class ContactValidator : IContactValidator
    {
        private readonly IActionsService _actionsService;
        private readonly ICustomFieldsService _customFieldsService;

        public ContactValidator(ICustomFieldsService customFieldsService, IActionsService actionsService)
        {
            _customFieldsService = customFieldsService;
            _actionsService = actionsService;
        }

        public async Task<string> ValidateContact(ContactDto contactDto)
        {
            // Check customfields
            foreach (var customFieldValue in contactDto.CustomFieldValues)
            {
                var customField = await _customFieldsService.GetById(customFieldValue.CfId);
                if (customField == null)
                    return $"CustomField with ID #{customFieldValue.CfId} not found!";
            }
            // Check actions
            foreach (var actionValue in contactDto.ActionValues)
            {
                var action = await _actionsService.GetById(actionValue.AId);
                if (action == null)
                    return $"Action with ID #{actionValue.AId} not found!";
            }
            return string.Empty;
        }
    }
}